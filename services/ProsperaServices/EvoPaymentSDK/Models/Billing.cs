using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models;

public class Billing

{
    [JsonPropertyName("address")]
    public string Address { get; set; }
    [JsonPropertyName("correlationId")]
    public string CorrelationId { get; set; }
}