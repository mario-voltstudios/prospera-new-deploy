using System.Text.Json.Serialization;
using AIProcess.Enuns;

namespace AIProcess.Models;

public class PaycheckResult :BaseClasse<PaycheckResult>
{
    [JsonPropertyName("matricula")]
    public  string Registration { get; set; }
    [JsonPropertyName("RFC")] 
    public string RFC { get; set; }
    [JsonPropertyName("nombre")]
    public  string Name { get; set; }
    [JsonPropertyName("curp")]
    public string Curp { get; set; }
    [JsonPropertyName("tipo_de_contratacion")]
    public HiringType HiringType { get; set; }
    [JsonPropertyName("clave_est_org")]
    public string OrgKey { get; set; }

    public Delegations Type
    {
        get
        {
            var first2Chars = OrgKey.Length >= 2 ? OrgKey[..2] : "";
            return (Delegations)int.Parse(first2Chars);
        }
    }

    [JsonPropertyName("percepciones")] 
    public List<DateEntry> Perceptions { get; set; } = [];

    [JsonPropertyName("deducciones")] 
    public List<DateEntry> Deductions { get; set; } = [];
    [JsonPropertyName("observaciones")]
    public List<Observation> Observations { get; set; } = [];
    [JsonPropertyName("contains_gnp_policy")]
    public bool HasGnpPolicy { get; set; }
}

public class DateEntry
{
    [JsonPropertyName("concepto")]
    public string Concept { get; set; }
    [JsonPropertyName("descripcion")]
    public string Description { get; set; }
    [JsonPropertyName("importe")]
    public double Amount { get; set; }
}
public class Observation
{
    [JsonPropertyName("concepto")]
    public string Concept { get; set; }
    [JsonPropertyName("importe")]
    public double Amount { get; set; }
    [JsonPropertyName("vencimento")]
    public string expiration { get; set; }
    [JsonPropertyName("unidades")]
    public string Units { get; set; }
    [JsonPropertyName("num_control")]
    public string ControlNumber { get; set; }
    [JsonPropertyName("observaciones")]
    public string Observations { get; set; }
}