using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;
using EvoPaymentSDK.Models.Responses.Base;

namespace EvoPaymentSDK.Models.Responses;

public class SessionResponse : BaseResponse
{
    [JsonPropertyName("session")]
    public Session Session { get; set; } = default!;
}