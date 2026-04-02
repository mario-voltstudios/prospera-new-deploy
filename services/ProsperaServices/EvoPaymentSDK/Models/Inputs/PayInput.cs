using System.Text.Json.Serialization;
using EvoPaymentSDK.Models.Inputs.Base;

namespace EvoPaymentSDK.Models.Inputs;

public class PayInput : BaseTransactionInput
{
    [JsonPropertyName("apiOperation")] 
    public string ApiOperation => "PAY";
}