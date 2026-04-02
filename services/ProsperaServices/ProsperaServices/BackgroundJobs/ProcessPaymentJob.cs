using ProsperaServices.Interfaces.Payment;
using ProsperaServices.Models.Supabase;
using ProsperaServices.PaymentGateway.EvoPayment;
using ProsperaServices.Supabase;
using TickerQ.Utilities.Base;
using PaymentInput = ProsperaServices.PaymentGateway.EvoPayment.PaymentInput;

namespace ProsperaServices.BackgroundJobs;

public sealed class ProcessPaymentJob(
    ILogger<ProcessPaymentJob> logger,
    IEnumerable<IPaymentGateway> paymentGateways,
    SupabaseService supabaseService)
{
    private const int MaxRetries = 3;
    private const int PendingThresholdMinutes = 5;

    [TickerFunction(nameof(ProcessPaymentJob), "*/5 * * * *")]
    public async Task Execute(TickerFunctionContext context, CancellationToken cancellationToken)
    {
        context.CronOccurrenceOperations.SkipIfAlreadyRunning();

        var evoHandler = (EvoPaymentHandler)paymentGateways.First(x => x is EvoPaymentHandler);

        try
        {
            // Query pending transactions older than 5 minutes with retry_count < 3
            var thresholdTime = DateTime.UtcNow.AddMinutes(-PendingThresholdMinutes);

            var result = await supabaseService.Client
                .From<PaymentTransaction>()
                .Where(x => x.Status == "pending")
                .Where(x => x.RetryCount < MaxRetries)
                .Where(x => x.CreatedAt < thresholdTime)
                .Where(x => x.PaymentGateway == "evopayment")
                .Get();

            var pendingTransactions = result?.Models?.ToList() ?? [];

            if (pendingTransactions.Count == 0)
            {
                logger.LogDebug("ProcessPaymentJob: No pending transactions to retry");
                return;
            }

            logger.LogInformation("ProcessPaymentJob: Found {Count} pending transactions to retry", pendingTransactions.Count);

            foreach (var transaction in pendingTransactions)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                await RetryTransaction(evoHandler, transaction, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ProcessPaymentJob: Error executing payment retry job");
        }
    }

    private async Task RetryTransaction(EvoPaymentHandler evoHandler, PaymentTransaction transaction, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation(
                "ProcessPaymentJob: Retrying transaction id={Id}, orderRef={OrderRef}, attempt={Attempt}/{Max}",
                transaction.Id, transaction.OrderReference, transaction.RetryCount + 1, MaxRetries);

            // Parse token data from stored transaction
            // Format: "type:data" where type is "CardToken" or "SessionId"
            var (tokenType, tokenData) = ParseTokenData(transaction.TokenData);

            var input = new PaymentInput
            {
                CustomerId = transaction.CustomerId ?? string.Empty,
                OrderId = transaction.OrderReference,
                TransactionId = transaction.TransactionReference ?? transaction.OrderReference,
                ReferenceId = transaction.ReciboNumber ?? transaction.OrderReference,
                Amount = (double)transaction.Amount,
                token = (tokenType, tokenData)
            };

            var result = await evoHandler.ChargeAsync(input, cancellationToken);

            // Increment retry count regardless of outcome
            transaction.RetryCount++;
            transaction.UpdatedAt = DateTime.UtcNow;

            // Check if charge succeeded or failed
            var (chargeData, error) = result;
            if (error is not null)
            {
                // Charge failed — update status to failed only if max retries reached
                transaction.ErrorMessage = $"{error.Title}: {error.Description}";

                if (transaction.RetryCount >= MaxRetries)
                {
                    transaction.Status = "failed_max_retries";
                    logger.LogWarning(
                        "ProcessPaymentJob: Transaction id={Id} reached max retries ({Max}), marked as failed_max_retries",
                        transaction.Id, MaxRetries);
                }
                else
                {
                    // Keep as pending for next retry
                    logger.LogInformation(
                        "ProcessPaymentJob: Transaction id={Id} retry {Retry} failed, will retry later. Error: {Error}",
                        transaction.Id, transaction.RetryCount, error.Title);
                }
            }
            else if (chargeData is not null)
            {
                // Charge succeeded
                transaction.Status = "success";
                transaction.CompletedAt = DateTime.UtcNow;
                transaction.ErrorMessage = null;

                logger.LogInformation(
                    "ProcessPaymentJob: Transaction id={Id} succeeded on retry {Retry}. OrderId={OrderId}",
                    transaction.Id, transaction.RetryCount, chargeData.OrderId);
            }

            // Update the transaction in Supabase
            await supabaseService.Client
                .From<PaymentTransaction>()
                .Update(transaction);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ProcessPaymentJob: Error retrying transaction id={Id}", transaction.Id);

            // Increment retry count and persist error
            transaction.RetryCount++;
            transaction.UpdatedAt = DateTime.UtcNow;
            transaction.ErrorMessage = ex.Message;

            if (transaction.RetryCount >= MaxRetries)
            {
                transaction.Status = "failed_max_retries";
            }

            try
            {
                await supabaseService.Client
                    .From<PaymentTransaction>()
                    .Update(transaction);
            }
            catch (Exception updateEx)
            {
                logger.LogError(updateEx, "ProcessPaymentJob: Failed to update transaction id={Id} after error", transaction.Id);
            }
        }
    }

    /// <summary>
    /// Parses stored token data in format "type:data" or just "data" (defaults to CardToken).
    /// </summary>
    private static (PaymentInputType type, string data) ParseTokenData(string? tokenData)
    {
        if (string.IsNullOrEmpty(tokenData))
        {
            return (PaymentInputType.CardToken, string.Empty);
        }

        var colonIndex = tokenData.IndexOf(':');
        if (colonIndex > 0 && Enum.TryParse<PaymentInputType>(tokenData[..colonIndex], ignoreCase: true, out var type))
        {
            return (type, tokenData[(colonIndex + 1)..]);
        }

        // Default: assume card token
        return (PaymentInputType.CardToken, tokenData);
    }
}
