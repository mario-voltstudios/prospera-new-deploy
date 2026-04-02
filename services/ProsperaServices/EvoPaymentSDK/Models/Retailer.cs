using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models;

public class Retailer
{
    [JsonPropertyName("abbreviatedTradingName")]
    public string AbbreviatedTradingName { get; set; }
    [JsonPropertyName("merchantCategoryCode")]
    public string MerchantCategoryCode { get; set; }
    [JsonPropertyName("tradingName")]
    public string TradingName { get; set; }
}