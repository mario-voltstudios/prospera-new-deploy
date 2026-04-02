using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionType
{
    AUTHENTICATION,
    AUTHORIZATION,
    AUTHORIZATION_UPDATE,
    CAPTURE,
    CHARGEBACK,
    DISBURSEMENT,
    FUNDING,
    PAYMENT,
    REFUND,
    REFUND_REQUEST,
    VERIFICATION,
    VOID_AUTHORIZATION,
    VOID_CAPTURE,
    VOID_PAYMENT,
    VOID_REFUND
}