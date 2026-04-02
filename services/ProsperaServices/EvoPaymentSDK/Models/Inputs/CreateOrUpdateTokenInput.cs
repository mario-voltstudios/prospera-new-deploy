using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;

namespace EvoPaymentSDK.Models.Inputs;

public class CreateOrUpdateTokenInput
{
    [JsonPropertyName("billing")]
    public Billing? Billing { get; set; }
    [JsonPropertyName("customer")]
    public Customer? Customer { get; set; }
    [JsonPropertyName("device")]
    public Device? Device { get; set; }
    [JsonPropertyName("referenceOrderId")]
    public string? ReferenceOrderId { get; set; }
    [JsonPropertyName("responseControls")]
    public  ResponseControls? ResponseControls { get; set; }
    [JsonPropertyName("session")]
    public Session? Session { get; set; }
    [JsonPropertyName("shipping")]
    public Shipping? Shipping { get; set; }
    [JsonPropertyName("sourceOfFunds")]
    public SourceOfFunds? SourceOfFunds { get; set; }
    [JsonPropertyName("transaction")]
    public Transaction? Transaction { get; set; }
    [JsonPropertyName("verificationStrategy")]
    public VerificationStrategy? VerificationStrategy { get; set; }
}