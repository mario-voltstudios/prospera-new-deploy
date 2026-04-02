using ProsperaServices.Interfaces.Payment;

namespace ProsperaServices.PaymentGateway.EvoPayment;

public class PaymentInput : IPaymentInput
{
    public required string CustomerId { get; set; }
    public required string OrderId { get; set; }
    public required string TransactionId { get; set; }
    public required string ReferenceId { get; set; }
    public required (PaymentInputType type, string data) token { get; set; }
    public required double Amount { get; set; }
}

public enum PaymentInputType
{
    CardToken,
    SessionId,
}