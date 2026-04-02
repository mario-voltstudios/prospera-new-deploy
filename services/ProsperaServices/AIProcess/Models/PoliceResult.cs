using System.Text.Json.Serialization;

namespace AIProcess.Models;

public class PoliceResult : BaseClasse<PoliceResult>
{
    [JsonPropertyName("poliza_no")]
    public string PolicyNumber { get; set; }
    [JsonPropertyName("referencia_1")]
    public string Reference1 { get; set; }
    [JsonPropertyName("referencia_2")]
    public string Reference2 { get; set; }
    [JsonPropertyName("codigo_cliente")]
    public string ClientCode { get; set; }
    [JsonPropertyName("importe_a_pagar")]
    public double AmountToPay { get; set; }
    [JsonPropertyName("fecha_de_expiracion")]
    public DateTime ExpirationDate { get; set; }
    [JsonPropertyName("vigencia_desde")]
    public DateTime ValidityFrom { get; set; }
    [JsonPropertyName("vigencia_hasta")]
    public DateTime ValidityTo { get; set; }
    [JsonPropertyName("especificaciones_plan")]
    public string PlanSpecifications { get; set; }
    [JsonPropertyName("contratante")]
    public  HirerClass Hirer { get; set; }
    public class HirerClass
    {
        [JsonPropertyName("nombre")]
        public string Name { get; set; }
        [JsonPropertyName("direccion")]
        public string Address { get; set; }
        [JsonPropertyName("rfc")]
        public string RFC { get; set; }
    }
    [JsonPropertyName("asegurado")]
    public InsuredClass Insured { get; set; }

    public class InsuredClass
    {
        [JsonPropertyName("nombre")] 
        public string Name { get; set; }
        [JsonPropertyName("codido_clente")]
        public string ClientCode { get; set; }
    }
    [JsonPropertyName("coberturas")]
    public List<CoverageClass> Coverages { get; set; } = [];
    public class CoverageClass
    {
        [JsonPropertyName("descripcion")]
        public string Description { get; set; }
        [JsonPropertyName("proteccion_contratada")]
        public string Protection { get; set; }
        [JsonPropertyName("opcionrd_de_liquidacion")]
        public string? LiquidationOption { get; set; }
    }
}