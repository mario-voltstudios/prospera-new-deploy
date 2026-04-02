using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FundingMethod
{
    CHARGE,
    CREDIT,
    DEBIT,
    UNKNOWN
}