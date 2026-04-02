using System.Text.Json.Serialization;
using AIProcess.Enuns;

namespace AIProcess.Models;

public class IdResult : BaseClasse<IdResult>
{
    [JsonPropertyName("nombre")]
    public string Name { get; set; }
    [JsonPropertyName("apellido_paterno")]
    public string LastNamePaternal { get; set; }
    [JsonPropertyName("apellido_materno")]
    public string LastNameMaternal { get; set; }
    [JsonPropertyName("fecha_nacimiento")]
    public DateTime BirthDate { get; set; }
    [JsonPropertyName("domicilio")]
    public AddressClass Address { get; set; }
    public class AddressClass
    {
        [JsonPropertyName("calle")]
        public string Street { get; set; }
        [JsonPropertyName("numero_exterior")]
        public string ExteriorNumber { get; set; }
        [JsonPropertyName("numero_interior")]
        public string InteriorNumber { get; set; }
        [JsonPropertyName("colonia")]
        public string Neighborhood { get; set; }
        [JsonPropertyName("municipio")]
        public string Municipality { get; set; }
        [JsonPropertyName("estado")]
        public string State { get; set; }
        [JsonPropertyName("codigo_postal")]
        public string PostalCode { get; set; }
        [JsonPropertyName("esta_completado")]
        public bool IsCompleted { get; set; }
    }
    [JsonPropertyName("curp")]
    public string Curp { get; set; }
    [JsonPropertyName("sexo")]
    public Gender Gender { get; set; }
}

public class IdResultBack : BaseClasse<IdResultBack>
{
    [JsonPropertyName("idmex")]
    public string Id { get; set; }
}