using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Models;

public class Recipient
{
    [JsonPropertyName("account")]
    public Account Account { get; set; }
    [JsonPropertyName("address")]
    public Address Address { get; set; }
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    [JsonPropertyName("identification")]
    public  Identification Identification { get; set; }
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    [JsonPropertyName("middleName")]
    public string MiddleName { get; set; }
}