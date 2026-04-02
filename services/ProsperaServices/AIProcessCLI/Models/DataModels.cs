namespace AIProcessCLI.Models;

public record UnableToProcess(string? PolicyNumber, string? Rfc, string? Name, string Reason);
public record Processed(string? PolicyNumber, string? Rfc, string? FileName);
public record Completed(string? PolicyNumber, string? Rfc, string? Name, string? Liquidity, bool IsRisk);