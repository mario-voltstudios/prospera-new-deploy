using EvoPaymentSDK.Enuns;

namespace ProsperaServices.Modes;

public class ChargeResponse
{
    public string TransactionId { get; set; }
    public TransactionResult TrasactionStatus { get; set; }
    public string OrderId { get; set; }
    public string OrderStatus { get; set; }
    public string Token { get; set; }
    public string Reference { get; set; }
    public string Description { get; set; }
}