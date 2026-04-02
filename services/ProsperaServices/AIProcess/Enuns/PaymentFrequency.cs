using System.Text.Json.Serialization;

namespace AIProcess.Enuns;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentFrequency
{
    Quincenal,
    Mensua,
}