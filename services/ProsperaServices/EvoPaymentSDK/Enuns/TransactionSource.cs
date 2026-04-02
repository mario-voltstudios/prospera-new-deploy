using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionSource
{
    /// <summary>
    /// Transaction conducted via a call centre.
    /// </summary>
    CALL_CENTRE,

    /// <summary>
    /// Transaction where the card is presented to the merchant.
    /// </summary>
    CARD_PRESENT,

    /// <summary>
    /// Transaction conducted over the Internet.
    /// </summary>
    INTERNET,

    /// <summary>
    /// Transaction received by mail.
    /// </summary>
    MAIL_ORDER,

    /// <summary>
    /// Transaction initiated by you based on an agreement with the payer.
    /// For example, a recurring payment, installment payment, or account top-up.
    /// </summary>
    MERCHANT,

    /// <summary>
    /// Transaction received by mail or telephone.
    /// </summary>
    MOTO,

    /// <summary>
    /// Transaction where a non-card payment method is presented to the Merchant.
    /// </summary>
    PAYER_PRESENT,

    /// <summary>
    /// Transaction received by telephone.
    /// </summary>
    TELEPHONE_ORDER,

    /// <summary>
    /// Voice response transaction.
    /// </summary>
    VOICE_RESPONSE,
}