using System.Text.Json.Serialization;

namespace EvoPaymentSDK.Implementations;

public class PosTerminal
{
    [JsonPropertyName("serialNumber")]
    public string SerialNumber { get; set; }
}