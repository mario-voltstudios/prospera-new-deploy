using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models.Inputs;

public class CreateSessionInput
{
    [JsonPropertyName("correlationId")]
    public string? CorrelationId { get; set; }
    [JsonPropertyName("authenticationLimit")]
    public int? AuthenticationLimit { get; set; }
}