namespace ProsperaServices.Constants;

/// <summary>
/// Configuration constants for ID generation
/// </summary>
public static class IdConfiguration
{
    /// <summary>
    /// Default length for session IDs
    /// 12 characters = ~1 in 3.2 trillion collision probability (62^12)
    /// 14 characters = ~1 in 768 trillion collision probability (62^14)
    /// 16 characters = ~1 in 47 quadrillion collision probability (62^16)
    /// </summary>
    public const int SessionIdLength = 12;
    
    /// <summary>
    /// For ultra-secure IDs where collision must be nearly impossible
    /// </summary>
    public const int UltraSecureIdLength = 16;
}
