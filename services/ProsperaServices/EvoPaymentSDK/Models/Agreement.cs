using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;

namespace EvoPaymentSDK.Models;

public class Agreement
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("amountVariability")]
    public AmountVariability AmountVariability { get; set; }
    [JsonPropertyName("customData")]
    public string? CustomData { get; set; }
    [JsonPropertyName("expiryDate")]
    public string? ExpiryDate { get; set; }
    [JsonPropertyName("maximumAmountPerPayment")]
    public decimal? MaximumAmountPerPayment { get; set; }
    [JsonPropertyName("minimumAmountPerPayment")]
    public decimal? MinimumAmountPerPayment { get; set; }
    [JsonPropertyName("minimumDaysBetweenPayments")]
    public int? MinimumDaysBetweenPayments { get; set; }
    [JsonPropertyName("numberOfPayments")]
    public int? NumberOfPayments { get; set; }
    [JsonPropertyName("paymentFrequency")]
    public PaymentFrequency? PaymentFrequency { get; set; }
    [JsonPropertyName("retailer")]
    public Retailer? Retailer { get; set; }
    [JsonPropertyName("startDate")]
    public DateTime? StartDate { get; set; }
    [JsonPropertyName("type")]
    public AgreementType? Type { get; set; }
    [JsonPropertyName("session")]
    public Session? Session { get; set; }
    [JsonPropertyName("shipping")]
    public Shipping? Shipping { get; set; }
    [JsonPropertyName("sourceOfFunds")]
    public SourceOfFunds? SourceOfFunds { get; set; }
}