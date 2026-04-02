using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;

namespace EvoPaymentSDK.Models;

public class SourceOfFundsProvided
{
    [JsonPropertyName("ach")]
    public  SourceOfFundsProvidedAchClass SourceOfFundsProvidedAch { get; set; }
    
    public class SourceOfFundsProvidedAchClass
    {
        [JsonPropertyName("accountType")]
        public AccountType AccountType { get; set; }
        [JsonPropertyName("bankAccountHolder")]
        public string BankAccountHolder { get; set; }
        [JsonPropertyName("bankAccountNumber")]
        public string BankAccountNumber { get; set; }
        [JsonPropertyName("routingNumber")]
        public string RoutingNumber { get; set; }
        [JsonPropertyName("secCode")]
        public SecCode SecCode { get; set; }
    }
    
    [JsonPropertyName("card")]
    public sourceOfFundsProvidedCardClass SourceOfFundsProvidedCard { get; set; }

    public class sourceOfFundsProvidedCardClass
    {
        [JsonPropertyName("expiry")]
        public ExpiryClass Expiry { get; set; }
        public class ExpiryClass
        {
            [JsonPropertyName("year")]
            public int Year { get; set; }
            [JsonPropertyName("month")]
            public int Month { get; set; }
            public static implicit operator ExpiryClass(string s)
            {
                return new ExpiryClass
                {
                    Month = int.Parse(s.Substring(0,2)),
                    Year =  int.Parse(s.Substring(2,2)),
                };
            }
        }
        
        [JsonPropertyName("nameOnCard")]
        public string NameOnCard { get; set; }
        [JsonPropertyName("number")]
        public long Number { get; set; }
        [JsonPropertyName("securityCode")]
        public int SecurityCode { get; set; }
    }
}