using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;

namespace EvoPaymentSDK.Models;

public class Account
{
    [JsonPropertyName("fundingMethod")]
    public FundingMethod? FundingMethod { get; set; }
    [JsonPropertyName("identifier")]
    public string? Identifier { get; set; }
    [JsonPropertyName("identifierType")]
    public IdentifierType? IdentifierType { get; set; }
}