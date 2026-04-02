using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models;

public class Transaction
{
    [JsonPropertyName("currency")]
    public string Currency { get; set; }
}