using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VerificationStrategy
{
    ACCOUNT_UPDATER,
    ACQUIRER,
    BASIC,
    NONE,
}