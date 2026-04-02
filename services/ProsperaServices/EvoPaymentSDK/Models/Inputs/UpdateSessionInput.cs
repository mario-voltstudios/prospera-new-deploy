using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;

namespace EvoPaymentSDK.Models.Inputs;

public class UpdateSessionInput
{
    [JsonPropertyName("accountFunding")]
    public AccountFunding? AccountFunding { get; set; }
    [JsonPropertyName("agreement")]
    public Agreement? Agreement { get; set; }
    [JsonPropertyName("customer")]
    public Customer? Customer { get; set; }
    [JsonPropertyName("device")]
    public Device? Device { get; set; }
    [JsonPropertyName("order")]
    public Order? Order { get; set; }
    [JsonPropertyName("paymentType")]
    public PaymentType PaymentType { get; set; }
    [JsonPropertyName("sourceOfFunds")]
    public SourceOfFunds? SourceOfFunds { get; set; }
    [JsonPropertyName("transaction")]
    public Transaction? Transaction { get; set; }
    [JsonPropertyName("transactionSource")]
    public TransactionSource? TransactionSource { get; set; }
    [JsonPropertyName("verificationStrategy")]
    public VerificationStrategy? VerificationStrategy { get; set; }
}