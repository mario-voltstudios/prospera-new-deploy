using EvoPayment;
using EvoPayment.Models.Inputs;
using EvoPaymentSDK.Enuns;
using EvoPaymentSDK.Models;
using EvoPaymentSDK.Models.Inputs;
using EvoPaymentSDK.Models.Responses;
using ProsperaServices.Contracts;
using ProsperaServices.Interfaces.IoC;
using ProsperaServices.Interfaces.Payment;
using ProsperaServices.Modes;
using ProsperaServices.Modes.Errors.BaseError;
using ProsperaServices.Models.Supabase;
using ProsperaServices.PaymentGateway.EvoPayment;
using ProsperaServices.Supabase;

namespace ProsperaServices.ApplicationServices;

public class CreateInsuredService(
    ILogger<CreateInsuredService> logger,
    SupabaseService supabase,
    IEnumerable<IPaymentGateway> paymentGateways) : ITransientDependency
{
    public async Task<Result<object>> Execute(CreateCustomerInput input, CancellationToken cancellationToken)
    {
        // ── 1. Look up solicitud by sales code ──────────────────────
        var salesCode = input.SalesCode ?? input.PolicyNumber;

        if (string.IsNullOrWhiteSpace(salesCode))
        {
            return new Error
            {
                Title = "Missing Sales Code",
                Description = "A SalesCode (or PolicyNumber) is required to look up the solicitud."
            };
        }

        logger.LogInformation("Looking up solicitud by sales code: {SalesCode}", salesCode);

        var solicitud = await supabase.GetSolicitudByCode(salesCode, cancellationToken);
        if (solicitud is null)
        {
            logger.LogWarning("Solicitud not found for sales code: {SalesCode}", salesCode);
            return new Error
            {
                Title = "Solicitud Not Found",
                Description = $"No solicitud found for sales code: {salesCode}"
            };
        }

        var primaAmount = solicitud.PrimaAnualRiesgo ?? solicitud.PrimaBase ?? 0;
        if (primaAmount <= 0)
        {
            logger.LogWarning("Solicitud {Folio} has no prima amount", solicitud.Folio);
            return new Error
            {
                Title = "Invalid Prima",
                Description = $"Solicitud {solicitud.Folio} has no prima amount configured."
            };
        }

        logger.LogInformation("Found solicitud {Folio}, prima: {Prima}", solicitud.Folio, primaAmount);

        // ── 2. Look up poliza (if already emitted) ──────────────────
        Poliza? poliza = null;
        try
        {
            poliza = await supabase.GetPolizaBySolicitudId(solicitud.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Could not look up poliza for solicitud {Id}, continuing without it", solicitud.Id);
        }

        // ── 3. Create pending payment transaction ───────────────────
        var tx = new PaymentTransaction
        {
            OrderReference = $"{salesCode}_{DateTime.UtcNow:yyyyMMddHHmmss}",
            Amount = primaAmount,
            Currency = "MXN",
            Status = "pending",
            CustomerId = solicitud.ClaveAgente ?? "unknown",
            PaymentGateway = "evopayment"
        };

        long txId;
        try
        {
            txId = await supabase.CreatePaymentTransaction(tx, cancellationToken);
            logger.LogInformation("Created payment_transaction {TxId} for solicitud {Folio}", txId, solicitud.Folio);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create payment_transaction for solicitud {Folio}", solicitud.Folio);
            return new Error
            {
                Title = "Transaction Error",
                Description = "Could not create the payment transaction record."
            };
        }

        // ── 4. Process payment via EVO ──────────────────────────────
        var evoPaymentHandler = (EvoPaymentHandler)paymentGateways.First(x => x is EvoPaymentHandler);

        var orderId = $"{solicitud.Folio}_{DateTime.UtcNow:yyyyMMddHHmmss}";
        var transactionId = Guid.NewGuid().ToString();

        logger.LogInformation(
            "Charging EVO for solicitud {Folio}, amount {Amount}, sessionId {SessionId}",
            solicitud.Folio, primaAmount, input.SessionId);

        var chargeResult = await evoPaymentHandler.ChargeAsync(new PaymentInput
        {
            CustomerId = solicitud.ClaveAgente ?? "unknown",
            Amount = (double)primaAmount,
            token = (PaymentInputType.SessionId, input.SessionId),
            OrderId = orderId,
            TransactionId = transactionId,
            ReferenceId = solicitud.Folio
        }, cancellationToken);

        // ── 5. Update transaction with result ───────────────────────
        if (chargeResult.IsError)
        {
            var errorTitle = chargeResult.Error?.Title ?? "Unknown";
            var errorDesc = chargeResult.Error?.Description ?? "EVO charge failed";

            logger.LogError("EVO charge failed for solicitud {Folio}: {Title} - {Desc}",
                solicitud.Folio, errorTitle, errorDesc);

            await supabase.UpdatePaymentTransaction(txId, "failed",
                $"{errorTitle}: {errorDesc}", cancellationToken);

            return chargeResult.Error!;
        }

        // Success — update transaction
        await supabase.UpdatePaymentTransaction(txId, "completed", errorMsg: null, cancellationToken);

        logger.LogInformation(
            "Payment completed for solicitud {Folio}, orderId: {OrderId}, transactionId: {TxId}",
            solicitud.Folio,
            chargeResult.Data?.OrderId,
            chargeResult.Data?.TransactionId);

        // Return the charge response (includes orderId, transactionId, reference, etc.)
        return chargeResult.Data!;
    }
}
