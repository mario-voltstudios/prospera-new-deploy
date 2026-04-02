using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GatewayCode
    {
        ABORTED,
        ACQUIRER_SYSTEM_ERROR,
        APPROVED,
        APPROVED_AUTO,
        APPROVED_PENDING_SETTLEMENT,
        AUTHENTICATION_FAILED,
        AUTHENTICATION_IN_PROGRESS,
        BALANCE_AVAILABLE,
        BALANCE_UNKNOWN,
        BLOCKED,
        CANCELLED,
        DECLINED,
        DECLINED_AVS,
        DECLINED_AVS_CSC,
        DECLINED_CSC,
        DECLINED_DO_NOT_CONTACT,
        DECLINED_INVALID_PIN,
        DECLINED_PAYMENT_PLAN,
        DECLINED_PIN_REQUIRED,
        DEFERRED_TRANSACTION_RECEIVED,
        DUPLICATE_BATCH,
        EXCEEDED_RETRY_LIMIT,
        EXPIRED_CARD,
        INSUFFICIENT_FUNDS,
        INVALID_CSC,
        LOCK_FAILURE,
        NOT_ENROLLED_3D_SECURE,
        NOT_SUPPORTED,
        NO_BALANCE,
        PARTIALLY_APPROVED,
        PENDING,
        REFERRED,
        SUBMITTED,
        SYSTEM_ERROR,
        TIMED_OUT,
        UNKNOWN,
        UNSPECIFIED_FAILURE,
        NO_VERIFICATION_PERFORMED,
        BASIC_VERIFICATION_SUCCESSFUL,
    }
}