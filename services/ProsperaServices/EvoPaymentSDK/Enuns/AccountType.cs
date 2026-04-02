using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountType
{
    CONSUMER_CHECKING,
    CONSUMER_SAVINGS,
    CORPORATE_CHECKING,
}