using ProsperaServices.Interfaces.Payment;

namespace ProsperaServices.PaymentGateway.Stripe;

public class PaymentInput : IPaymentInput
{
    public string clientId { get; set; }
    public string cardId { get; set; }
    public double Amount { get; set; }
}