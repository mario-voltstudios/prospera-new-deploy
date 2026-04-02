namespace ProsperaServices.Modes.Errors.BaseError;

public class Error
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public Exception? Exception { get; init; }
}