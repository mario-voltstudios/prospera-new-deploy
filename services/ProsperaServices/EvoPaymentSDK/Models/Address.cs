using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models;

public class Address
{
    [JsonPropertyName("city")]
    public string City { get; set; }
    [JsonPropertyName("company")]
    public string Company { get; set; }
    [JsonPropertyName("country")]
    public string Country { get; set; }
    [JsonPropertyName("postCodeZip")]
    public string PostCodeZip { get; set; }
    [JsonPropertyName("stateProvince")]
    public string StateProvince { get; set; }
    [JsonPropertyName("street")]
    public string Street { get; set; }
    [JsonPropertyName("street2")]
    public string Street2 { get; set; }
}