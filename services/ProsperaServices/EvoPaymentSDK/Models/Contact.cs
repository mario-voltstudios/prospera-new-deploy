using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models;

public class Contact
{
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
}