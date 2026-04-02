using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models.Responses;

public class OrderResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("creationTime")]
    public DateTime CreationTime { get; set; }
    
    [JsonPropertyName("currency")]
    public string Currency { get; set; }
    
    [JsonPropertyName("reference")]
    public  string Reference { get; set; }
    
    [JsonPropertyName("status")]
    public  string Status { get; set; }
    
    [JsonPropertyName("lastUpdatedTime")]
    public DateTime LastUpdatedTime { get; set; }
    
    [JsonPropertyName("merchantAmount")]
    public decimal MerchantAmount { get; set; }
    
    [JsonPropertyName("merchantCurrency")]
    public string MerchantCurrency { get; set; }
    
    [JsonPropertyName("totalAuthorizedAmount")]
    public decimal TotalAuthorizedAmount { get; set; }
    
    [JsonPropertyName("totalCapturedAmount")]
    public decimal TotalCapturedAmount { get; set; }
    
    [JsonPropertyName("totalDisbursedAmount")]
    public decimal TotalDisbursedAmount { get; set; }
}