using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models.Inputs.Base;

public class BaseTransactionInput
{
    [JsonPropertyName("accountFunding")]
    public AccountFunding? AccountFunding { get; set; }
    [JsonPropertyName("agreement")]
    public Agreement? Agreement { get; set; }
    [JsonPropertyName("order")]
    public Order? Order { get; set; }
    [JsonPropertyName("sourceOfFunds")]
    public SourceOfFunds SourceOfFunds { get; set; }
    [JsonPropertyName("session")]
    public Session? Session { get; set; }
}