using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models.Responses;

public class TransactionResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
    [JsonPropertyName("currency")]
    public string Currency { get; set; }
}