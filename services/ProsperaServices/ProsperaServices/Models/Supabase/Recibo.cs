namespace ProsperaServices.Models.Supabase;

/// <summary>
/// Maps to the <c>recibos</c> table in Supabase.
/// Partial — only fields needed by the payment API.
/// </summary>
public class Recibo
{
    public Guid Id { get; set; }
    public Guid? PolizaId { get; set; }
    public DateTime? PeriodoFecha { get; set; }
    public string? PeriodoLabel { get; set; }
    public decimal? Monto { get; set; }
    public decimal? MontoLiquidado { get; set; }
    public string Status { get; set; } = "Pendiente";
    public DateTime? FechaLiquidacion { get; set; }
    public string? ContratanteNombre { get; set; }
    public string? AgenteClave { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
