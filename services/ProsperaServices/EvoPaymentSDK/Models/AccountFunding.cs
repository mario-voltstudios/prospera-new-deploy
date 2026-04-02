using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;

namespace EvoPaymentSDK.Models;

public class AccountFunding
{
    [JsonPropertyName("purpose")]
    public AccountFundingPurpose Purpose { get; set; }
    [JsonPropertyName("recipient")]
    public Recipient Recipient { get; set; }
    [JsonPropertyName("senderIsRecipient")]
    public bool SenderIsRecipient { get; set; }
    [JsonPropertyName("senderType")]
    public SenderType SenderType { get; set; }
}