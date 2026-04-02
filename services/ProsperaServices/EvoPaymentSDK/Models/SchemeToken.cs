using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models;

public class SchemeToken
{
    [JsonPropertyName("status")]
    public string Status { get; set; }
    [JsonPropertyName("statusTime")]
    public DateTime StatusTime { get; set; }
}