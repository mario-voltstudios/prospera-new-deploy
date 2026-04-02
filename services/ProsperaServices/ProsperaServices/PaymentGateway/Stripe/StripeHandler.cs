using ProsperaServices.Interfaces.Payment;
using ProsperaServices.Modes;

namespace ProsperaServices.PaymentGateway.Stripe;

public class StripeHandler(ILogger<StripeHandler> logger) : IPaymentGateway<Stripe.PaymentInput>
{
    public Task<Result<ChargeResponse>>  ChargeAsync(PaymentInput input, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}