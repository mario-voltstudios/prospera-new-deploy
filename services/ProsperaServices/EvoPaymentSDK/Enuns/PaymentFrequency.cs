using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentFrequency
{
    AD_HOC,
    DAILY,
    FORTNIGHTLY,
    MONTHLY,
    OTHER,
    QUARTERLY,
    TWICE_YEARLY,
    WEEKLY,
    YEARLY,
}