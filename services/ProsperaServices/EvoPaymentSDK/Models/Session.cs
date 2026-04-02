using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;

namespace EvoPaymentSDK.Models;

public class Session
{
    [JsonPropertyName("id")]
    public string SessionId { get; set; }
    [JsonPropertyName("version")]
    public string SessionVersion { get; set; }
    // [JsonPropertyName("updateStatus")]
    // public UpdateStatus UpdateStatus { get; set; }
    // [JsonPropertyName("authenticationLimit")]
    // public int AuthenticationLimit { get; set; }
    // [JsonPropertyName("aes256Key")]
    // public string Aes256Key { get; set; }
}