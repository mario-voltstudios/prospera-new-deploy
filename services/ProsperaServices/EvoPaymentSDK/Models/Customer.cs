using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models;

public class Customer
{
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    [JsonPropertyName("mobilePhone")]
    public string MobilePhone { get; set; }
    [JsonPropertyName("phone")]
    public string Phone { get; set; }
    [JsonPropertyName("taxRegistrationId")]
    public string TaxRegistrationId { get; set; }
}