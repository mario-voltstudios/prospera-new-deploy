using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;

namespace EvoPayment.Implementations;

public class EvoError
{
    [JsonPropertyName("error")]
    public ErrorDetails Error { get; set; }
    [JsonPropertyName("result")]
    public  string Result { get; set; }
    
    public class ErrorDetails
    {
        [JsonPropertyName("cause")]
        public ErrorCause Cause { get; set; }
        [JsonPropertyName("explanation")]
        public string Explanation { get; set; }
        [JsonPropertyName("Field")]
        public string Field { get; set; }
        [JsonPropertyName("supportCode")]
        public string SupportCode { get; set; }
        [JsonPropertyName("validationType")]
        public ValidationType ValidationType { get; set; }
    }
}