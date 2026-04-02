using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SenderType
{
    COMMERCIAL_ORGANIZATION,
    GOVERNMENT,
    NON_PROFIT_ORGANIZATION,
    PERSON
}