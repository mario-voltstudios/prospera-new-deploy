using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorCause
{
    INVALID_REQUEST,
    REQUEST_REJECTED,
    SERVER_BUSY,
    SERVER_FAILED
}