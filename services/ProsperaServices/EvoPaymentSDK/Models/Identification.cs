using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;

namespace EvoPaymentSDK.Models;

public class Identification
{
    [JsonPropertyName("country")]
    public string Country { get; set; }
    [JsonPropertyName("type")]
    public IndentificationType Type { get; set; }
    [JsonPropertyName("value")]
    public string Value { get; set; }
}