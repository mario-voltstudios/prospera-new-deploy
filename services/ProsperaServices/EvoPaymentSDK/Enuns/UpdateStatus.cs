using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UpdateStatus
{
    FAILURE,
    NO_UPDATE,
    SUCCESS
}