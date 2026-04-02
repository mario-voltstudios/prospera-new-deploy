namespace ProsperaServices.Models.Supabase;

/// <summary>
/// Maps to the <c>polizas</c> table in Supabase (new native table, not legacy <c>polices</c>).
/// </summary>
public class Poliza
{
    public Guid Id { get; set; }
    public Guid? SolicitudId { get; set; }
    public string? NumPoliza { get; set; }
    public string? NumCertificado { get; set; }
    public DateTime? FechaEmision { get; set; }
    public DateTime? FechaVigenciaIni { get; set; }
    public DateTime? FechaVigenciaFin { get; set; }
    public string? EmisorClave { get; set; }
    public string Status { get; set; } = "pendiente";
    public string? GnpContratanteNombre { get; set; }
    public string? GnpContratanteRfc { get; set; }
    public string? GnpAseguradoNombre { get; set; }
    public string? GnpPlan { get; set; }
    public decimal? GnpPrimaMensual { get; set; }
    public decimal? GnpSumaAsegurada { get; set; }
    public bool Paso2Completado { get; set; }
    public bool Paso25Verificado { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
