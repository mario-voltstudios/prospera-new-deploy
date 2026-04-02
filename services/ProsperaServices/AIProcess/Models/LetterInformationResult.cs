using System.Text.Json.Serialization;
using AIProcess.Enuns;

namespace AIProcess.Models;

public class LetterInformationResult : BaseClasse<LetterInformationResult>
{
    [JsonPropertyName("apellido_paterno")]
    public string? LastNamePaternal { get; set; }
    [JsonPropertyName("apellido_materno")]
    public string? LastNameMaternal { get; set; }
    [JsonPropertyName("nombre")]
    public string? Name { get; set; }
    [JsonPropertyName("fecha")]
    public DateTime? Date { get; set; }
    [JsonPropertyName("matricula")]
    public string? RegistrationNumber { get; set; }
    [JsonPropertyName("delegacion")]
    public string? Delegation { get; set; }
    [JsonPropertyName("clave")]
    public string? Key { get; set; }
    [JsonPropertyName("tipo")]
    public TypeOfWorker? Type { get; set; }
    [JsonPropertyName("plan")]
    public string? Plan { get; set; }
    [JsonPropertyName("ramo")]
    public string? Branch { get; set; }
    [JsonPropertyName("monto_anual_prima")]
    public double? AnnualAmountPrime { get; set; }
    [JsonPropertyName("inporte_retencion")]
    public double? RetentionAmount { get; set; }
    [JsonPropertyName("frecuencia_pago")]
    public PaymentFrequency? PaymentFrequency { get; set; }
    [JsonPropertyName("importe_suma_asegurada")]
    public double? InsuredSumAmount { get; set; }
}