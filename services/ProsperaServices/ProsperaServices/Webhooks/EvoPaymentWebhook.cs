using System.Text.Json;
using System.Text.Json.Serialization;
using EvoPayment;
using EvoPayment.Models.Inputs;
using EvoPaymentSDK.Enuns;
using EvoPaymentSDK.Implementations;
using ProsperaServices.Interfaces.Webhooks;
using ProsperaServices.Supabase;
using Serilog;
using Serilog.Events;
using SerilogTracing;
using ILogger = Serilog.ILogger;

namespace ProsperaServices.Webhooks;

public class EvoPaymentWebhook(
    ILogger logger,
    IDiagnosticContext diagnosticContext,
    IConfiguration configuration,
    SupabaseService supabaseService
) : IWebhookHandler
{
    public string Name => "evopayment";
    public string Methods => HttpMethods.Post;

    public async Task Handle(HttpContext context)
    {
        using var activity = logger.StartActivity(LogEventLevel.Information, "EvoPaymentWebhookHandler");
        diagnosticContext.Set("Webhook", "EvoPayment");

        try
        {
            // 1. Read and parse the request body
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            logger.Information("EvoPayment webhook received: {Body}", body);

            if (string.IsNullOrWhiteSpace(body))
            {
                logger.Warning("EvoPayment webhook received empty body");
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("empty body");
                return;
            }

            var notification = JsonSerializer.Deserialize<WebhookNotification>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });

            if (notification is null)
            {
                logger.Warning("EvoPayment webhook failed to deserialize notification");
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("invalid payload");
                return;
            }

            // 2. Extract transaction reference and status
            var orderId = notification.Order?.Id;
            var transactionId = notification.Transaction?.Id;
            var gatewayCode = notification.Response?.GatewayCode;
            var orderStatus = notification.Order?.Status;

            logger.Information(
                "EvoPayment webhook: OrderId={OrderId}, TransactionId={TransactionId}, GatewayCode={GatewayCode}, OrderStatus={OrderStatus}",
                orderId, transactionId, gatewayCode, orderStatus);

            // Determine our mapped status
            var mappedStatus = MapGatewayCodeToStatus(gatewayCode, orderStatus);
            logger.Information("Mapped status: {Status} for OrderId={OrderId}", mappedStatus, orderId);

            // 3. Update payment_transactions in Supabase
            if (!string.IsNullOrEmpty(orderId))
            {
                await UpdatePaymentTransaction(orderId, transactionId, mappedStatus, gatewayCode);
            }
            else if (!string.IsNullOrEmpty(transactionId))
            {
                await UpdatePaymentTransaction(transactionId, transactionId, mappedStatus, gatewayCode);
            }
            else
            {
                logger.Warning("EvoPayment webhook: no orderId or transactionId found in notification");
            }

            // 4. If success, mark recibo as cobrado
            if (mappedStatus == "success")
            {
                var referenceId = notification.Order?.Reference;
                if (!string.IsNullOrEmpty(referenceId))
                {
                    await MarkReciboAsCobrado(referenceId, orderId);
                }
                else
                {
                    logger.Warning("EvoPayment webhook success but no reference found to mark recibo as cobrado");
                }
            }

            // 5. Return 200 OK
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("ok");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "EvoPayment webhook handler error");
            // Still return 200 to avoid Mastercard Gateway retries on our internal errors
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("ok");
        }
    }

    /// <summary>
    /// Maps Mastercard Gateway codes to our internal status strings.
    /// CAPTURED/SUCCESS → "success"
    /// DECLINED/FAILED/ERROR → "failed"
    /// PENDING/SUBMITTED → "pending"
    /// </summary>
    private static string MapGatewayCodeToStatus(GatewayCode? gatewayCode, string? orderStatus)
    {
        // Normalize: prefer gatewayCode, fall back to orderStatus string comparison
        if (gatewayCode.HasValue)
        {
            return gatewayCode.Value switch
            {
                GatewayCode.APPROVED or GatewayCode.APPROVED_AUTO or GatewayCode.APPROVED_PENDING_SETTLEMENT
                    => "success",
                GatewayCode.DECLINED or GatewayCode.DECLINED_AVS or GatewayCode.DECLINED_CSC
                    or GatewayCode.DECLINED_AVS_CSC or GatewayCode.DECLINED_DO_NOT_CONTACT
                    or GatewayCode.DECLINED_INVALID_PIN or GatewayCode.DECLINED_PAYMENT_PLAN
                    or GatewayCode.DECLINED_PIN_REQUIRED or GatewayCode.EXPIRED_CARD
                    or GatewayCode.INSUFFICIENT_FUNDS or GatewayCode.INVALID_CSC
                    or GatewayCode.AUTHENTICATION_FAILED or GatewayCode.BLOCKED
                    or GatewayCode.CANCELLED or GatewayCode.EXCEEDED_RETRY_LIMIT
                    or GatewayCode.SYSTEM_ERROR or GatewayCode.TIMED_OUT
                    or GatewayCode.ABORTED or GatewayCode.UNSPECIFIED_FAILURE
                    or GatewayCode.UNKNOWN
                    => "failed",
                GatewayCode.PENDING or GatewayCode.SUBMITTED
                    or GatewayCode.AUTHENTICATION_IN_PROGRESS or GatewayCode.DEFERRED_TRANSACTION_RECEIVED
                    => "pending",
                _ => "pending"
            };
        }

        // Fallback to orderStatus string matching (Mastercard Gateway order status values)
        if (!string.IsNullOrEmpty(orderStatus))
        {
            var status = orderStatus.ToUpperInvariant();
            return status switch
            {
                "CAPTURED" or "COMPLETED" => "success",
                "DECLINED" or "FAILED" or "ERROR" or "CANCELLED" => "failed",
                "PENDING" or "PROCESSING" or "SUBMITTED" => "pending",
                _ => "pending"
            };
        }

        return "pending";
    }

    /// <summary>
    /// Updates the payment_transactions row in Supabase matching the given order/transaction reference.
    /// </summary>
    private async Task UpdatePaymentTransaction(string orderReference, string? transactionId, string status, GatewayCode? gatewayCode)
    {
        try
        {
            logger.Information(
                "Updating payment_transaction: reference={Reference}, status={Status}",
                orderReference, status);

            // Find the transaction by order reference (order_id or transaction_reference)
            var result = await supabaseService.Client
                .From<Models.Supabase.PaymentTransaction>()
                .Where(x => x.OrderReference == orderReference)
                .Get();

            var transaction = result?.Models?.FirstOrDefault();

            if (transaction is null)
            {
                // Also try by transaction_id if provided and different
                if (!string.IsNullOrEmpty(transactionId) && transactionId != orderReference)
                {
                    result = await supabaseService.Client
                        .From<Models.Supabase.PaymentTransaction>()
                        .Where(x => x.OrderReference == transactionId)
                        .Get();
                    transaction = result?.Models?.FirstOrDefault();
                }
            }

            if (transaction is null)
            {
                logger.Warning("No payment_transaction found for reference={Reference}", orderReference);
                return;
            }

            transaction.Status = status;
            transaction.GatewayCode = gatewayCode?.ToString();
            transaction.UpdatedAt = DateTime.UtcNow;

            if (status == "success")
            {
                transaction.CompletedAt = DateTime.UtcNow;
            }

            await supabaseService.Client
                .From<Models.Supabase.PaymentTransaction>()
                .Update(transaction);

            logger.Information("Updated payment_transaction id={Id} to status={Status}", transaction.Id, status);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error updating payment_transaction for reference={Reference}", orderReference);
        }
    }

    /// <summary>
    /// Marks a recibo as cobrado (paid) in the receipts table when payment succeeds.
    /// </summary>
    private async Task MarkReciboAsCobrado(string reference, string? orderId)
    {
        try
        {
            // Reference format from EvoPaymentHandler: "{referenceId}_{date}"
            // Extract the recibo number (referenceId part before the date suffix)
            var reciboRef = reference.Contains('_') ? reference[..reference.LastIndexOf('_')] : reference;

            logger.Information("Marking recibo {Recibo} as cobrado (orderId={OrderId})", reciboRef, orderId);

            // Insert into paid_receipt table to mark as cobrado
            var paidReceipt = new Models.Supabase.PaidReceipt
            {
                ReceiptNumber = reciboRef,
                PaidAt = DateTime.UtcNow,
                OrderReference = orderId ?? string.Empty,
                PaymentMethod = "evopayment"
            };

            await supabaseService.Client
                .From<Models.Supabase.PaidReceipt>()
                .Insert(paidReceipt);

            // Also update the receipt status in the receipts table
            var receiptResult = await supabaseService.Client
                .From<Models.Supabase.Receipt>()
                .Where(x => x.NumeroRecibo == reciboRef)
                .Get();

            var receipt = receiptResult?.Models?.FirstOrDefault();
            if (receipt is not null)
            {
                receipt.EstatusRecibo = "COBRADO";
                receipt.FechaCobro = DateTime.UtcNow;

                await supabaseService.Client
                    .From<Models.Supabase.Receipt>()
                    .Update(receipt);

                logger.Information("Updated receipt {Recibo} status to COBRADO", reciboRef);
            }
            else
            {
                logger.Warning("Receipt not found for recibo={Recibo}, inserted paid_receipt only", reciboRef);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error marking recibo as cobrado for reference={Reference}", reference);
        }
    }
}
