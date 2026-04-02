using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;

namespace EvoPaymentSDK.Models.Responses;

public class CreateOrUpdateTokenResponse
{
    [JsonPropertyName("repositoryId")]
    public  string RepositoryId { get; set; }
    [JsonPropertyName("result")]
    public TransactionResult Result { get; set; }
    [JsonPropertyName("response")]
    public Response Response { get; set; }
    [JsonPropertyName("schemeToken")]
    public SchemeToken SchemeToken { get; set; }
    // [JsonPropertyName("sourceOfFunds")]
    // public  SourceOfFunds SourceOfFunds { get; set; }
    [JsonPropertyName("status")]
    public string Status { get; set; }
    [JsonPropertyName("token")]
    public  string Token { get; set; }
    [JsonPropertyName("usage")]
    public Usage Usage { get; set; }
    [JsonPropertyName("verificationStrategy")]
    public VerificationStrategy VerificationStrategy { get; set; }
}