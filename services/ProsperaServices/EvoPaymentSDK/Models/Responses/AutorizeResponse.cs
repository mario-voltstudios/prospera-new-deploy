using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;
using EvoPaymentSDK.Models.Responses.Base;

namespace EvoPaymentSDK.Models.Responses;

public class AutorizeResponse : BaseResponse
{
    [JsonPropertyName("order")]
    public OrderResponse OrderResponse { get; set; }  = default!;
    [JsonPropertyName("response")]
    public Response Response { get; set; } = default!;
    [JsonPropertyName("transaction")]
    public TransactionResponse TransactionResponse { get; set; }  = default!;
}