using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionResult
{
    FAILURE,
    PENDING,
    SUCCESS,
    UNKNOWN
}