namespace ProsperaServices.Contracts;

public record CreateCustomerInput(
    string FullName,
    string PolicyNumber,
    string SessionId,
    string? SalesCode = null);
