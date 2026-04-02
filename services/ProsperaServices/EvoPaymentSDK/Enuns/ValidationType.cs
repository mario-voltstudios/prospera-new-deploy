using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ValidationType
{
    INVALID,
    MISSING,
    UNSUPPORTED,
}