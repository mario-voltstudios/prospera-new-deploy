using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models;

public class Order
{
    [JsonPropertyName("amount")]
    public double Amount { get; set; }
    [JsonPropertyName("currency")]
    public string Currency { get; set; }
    // [JsonPropertyName("netAmount")]
    // public double NetAmount { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("reference")]
    public string Reference { get; set; }
}