using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountFundingPurpose
{
    CRYPTOCURRENCY_PURCHASE,
    MERCHANT_SETTLEMENT,
    OTHER,
    PAYROLL,
}