using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models;

public class ResponseControls
{
    [JsonPropertyName("sensitiveData")]
    public bool SensitiveData { get; set; }
}