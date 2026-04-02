using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;

namespace EvoPaymentSDK.Models.Responses.Base;

public class BaseResponse
{
    [JsonPropertyName("merchant")] 
    public string Merchant { get; set; } = default!;
    [JsonPropertyName("result")]
    public TransactionResult Result { get; set; }
}