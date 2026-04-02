using System.Text.Json.Serialization;
using EvoPayment.Models.Inputs;

namespace EvoPaymentSDK.Models;

public class Shipping
{
    [JsonPropertyName("address")]
    public Address Address { get; set; }
    [JsonPropertyName("contact")]
    public Contact Contact { get; set; }
    [JsonPropertyName("origin")]
    public OriginClass Origin { get; set; }
    
    public class OriginClass
    {
        [JsonPropertyName("postcode")]
        public string Postcode { get; set; }
    }
}