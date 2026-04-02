using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models;

public class Usage
{
    [JsonPropertyName("lastUpdated")]
    public LastUpdatedClass LastUpdated { get; set; }
    
    public class LastUpdatedClass
    {
        [JsonPropertyName("merchantId")]
        public string MerchantId { get; set; }
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
    }
    [JsonPropertyName("lastUsedTime")]
    public DateTime LastUsedTime { get; set; }
}