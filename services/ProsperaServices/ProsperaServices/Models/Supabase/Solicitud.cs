namespace ProsperaServices.Models.Supabase;

/// <summary>
/// Maps to the <c>solicitudes</c> table in Supabase.
/// Only fields used by the payment API are included; add more as needed.
/// </summary>
public class Solicitud
{
    public Guid Id { get; set; }
    public string Folio { get; set; } = "";
    public string? ClaveAgente { get; set; }
    public string Status { get; set; } = "pendiente";
    public string? ContratanteNombres { get; set; }
    public string? ContratanteApPaterno { get; set; }
    public string? ContratanteApMaterno { get; set; }
    public string? ContratanteEmail { get; set; }
    public string? ContratanteTelefono { get; set; }
    public string? Producto { get; set; }
    public string? Plan { get; set; }
    public decimal? PrimaBase { get; set; }
    public decimal? PrimaAdicional { get; set; }
    public decimal? PrimaAnualRiesgo { get; set; }
    public decimal? SumaAseguradaCotizada { get; set; }
    public string? EstadoVenta { get; set; }
    public string? Dependencia { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
