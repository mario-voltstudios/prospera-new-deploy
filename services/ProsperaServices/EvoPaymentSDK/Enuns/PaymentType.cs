using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Enuns;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentType
{
    ACH,
    ALIPAY,
    BANCANET,
    BANCONTACT,
    BLIK,
    BOLETO_BANCARIO,
    BROWSER_PAYMENT,
    CARD,
    DIRECT_DEBIT_CANADA,
    ENETS,
    EPS_UEBERWEISUNG,
    GIFT_CARD,
    GIROPAY,
    GRABPAY,
    IDEAL,
    KLARNA_FINANCING,
    KLARNA_PAY_LATER,
    KLARNA_PAY_NOW,
    MERCADO_PAGO_CHECKOUT,
    MULTIBANCO,
    OPEN_BANKING_BANK_TRANSFER,
    OXXO,
    PAYCONIQ,
    PAYPAL,
    PAYSAFECARD,
    PAYU,
    PBBA,
    POLI,
    PRZELEWY24,
    SEPA,
    SOFORT,
    TRUSTLY,
    UNION_PAY,
    WECHAT_PAY
}