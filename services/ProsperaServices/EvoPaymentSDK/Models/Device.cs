using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models;

public class Device
{
    [JsonPropertyName("ipAddress")]
    public string IpAddress { get; set; }
}