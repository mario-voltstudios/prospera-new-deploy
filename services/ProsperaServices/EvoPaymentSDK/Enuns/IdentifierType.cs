using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum IdentifierType
{
    BANK_ACCOUNT_BIC,
    BANK_ACCOUNT_IBAN,
    BANK_ACCOUNT_NATIONAL,
    CARD_NUMBER,
    EMAIL_ADDRESS,
    OTHER,
    PHONE_NUMBER,
    SOCIAL_NETWORK_PROFILE_ID,
    STAGED_WALLET_USER_ID,
    STORED_VALUE_WALLET_USER_ID,
}