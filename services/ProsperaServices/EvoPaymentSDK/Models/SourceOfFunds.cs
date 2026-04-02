using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;
using EvoPaymentSDK.Implementations;

namespace EvoPaymentSDK.Models;

public class SourceOfFunds
{
    [JsonPropertyName("posTerminal")]
    public PosTerminal PosTerminal { get; set; }
    [JsonPropertyName("provided")]
    public SourceOfFundsProvided Provided { get; set; }
    [JsonPropertyName("schemeTokenProvisioningIdentifier")]
    public string SchemeTokenProvisioningIdentifier { get; set; }
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [JsonPropertyName("tokenRequestorID")]
    public string TokenRequestorID { get; set; }
    [JsonPropertyName("type")]
    public SourceOfFundsType Type { get; set; }
}