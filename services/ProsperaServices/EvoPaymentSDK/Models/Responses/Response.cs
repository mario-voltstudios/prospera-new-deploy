using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;

namespace EvoPaymentSDK.Models.Responses;

public class Response
{
    [JsonPropertyName("gatewayCode")]
    public GatewayCode GatewayCode { get; set; }
}