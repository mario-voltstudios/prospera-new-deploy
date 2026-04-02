using System.Text.RegularExpressions;
using CsvHelper.Configuration.Attributes;

namespace AIProcessCLI.Models;

public partial class PolicesRetireesCsv
{
    [Name("Numero Poliza")]
    public string PolicyNumber { get; set; }
    [Name("Contratante Link")]
    public string ContractorLink { get; set; }
    [Name("Clave Agente")]
    public string AgentKey { get; set; }
    [Name("Numero Solicitud")]
    public string ApplicationNumber { get; set; }
    [Name("Fecha Emitido")]
    public DateTime? IssueDate { get; set; }
    [Name("Talones de Pago")]
    public string PaymentStubsLink { private get; set; }
    [Name("Prima Anual Seguro")]
    public string AnnualInsurancePremium { get; set; }
    [Name("Fecha Inicio Vigencia")]
    public DateTime? StartDate { get; set; }
    [Name("Prima Mensual de Riesgo")]
    public string MonthlyRiskPremium { get; set; }

    public (string link, string extension)? GetDownloadable()
    {
        if (string.IsNullOrEmpty(PaymentStubsLink)) return null;
        
        var match = ExtractLinkRegex().Match(PaymentStubsLink);

        if (!match.Success) return (null);
        
        var fileName = match.Groups["filename"].Value.Trim();
        var url = match.Groups["url"].Value;
            
        return (url, Path.GetExtension(fileName));

    }

    [GeneratedRegex(@"^(?<filename>[^()]+)\s*\((?<url>https?:\/\/[^)]+)\)$")]
    private static partial Regex ExtractLinkRegex();
}