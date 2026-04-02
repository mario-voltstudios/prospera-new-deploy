using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SourceOfFundsType
{
    ACH,
    CARD,
    DIRECT_DEBIT_CANADA,
    GIFT_CARD,
    PAYPAL,
}