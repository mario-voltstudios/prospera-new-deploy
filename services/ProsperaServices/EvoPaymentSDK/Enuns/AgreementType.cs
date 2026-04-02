using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AgreementType
{
    INSTALLMENT,
    OTHER,
    RECURRING,
    UNSCHEDULED,
}