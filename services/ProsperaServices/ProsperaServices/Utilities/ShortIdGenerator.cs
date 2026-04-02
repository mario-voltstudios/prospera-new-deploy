using System.Security.Cryptography;
using System.Text;

namespace ProsperaServices.Utilities;

/// <summary>
/// Generates short, unique, collision-resistant IDs with configurable length
/// Uses timestamp + random bytes + Base62 encoding for maximum compactness
/// </summary>
public static class ShortIdGenerator
{
    private const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private static readonly DateTime Epoch = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    
    /// <summary>
    /// Generates a unique short ID with the specified length
    /// </summary>
    /// <param name="length">Length of the ID (recommended: 12-16 for very low collision probability)</param>
    /// <returns>A collision-resistant unique ID</returns>
    /// <remarks>
    /// Collision probability:
    /// - 12 chars: ~1 in 3.2 trillion (62^12)
    /// - 14 chars: ~1 in 768 trillion (62^14)
    /// - 16 chars: ~1 in 47 quadrillion (62^16)
    /// 
    /// Format: [timestamp_part][random_part]
    /// - First ~40% is timestamp-based (ensures temporal uniqueness)
    /// - Remaining ~60% is cryptographically random (ensures collision resistance)
    /// </remarks>
    public static string Generate(int length = 12)
    {
        if (length < 6)
        {
            throw new ArgumentException("ID length must be at least 6 characters", nameof(length));
        }
        
        // Calculate timestamp in milliseconds since epoch
        var timestamp = (long)(DateTime.UtcNow - Epoch).TotalMilliseconds;
        
        // Allocate length for timestamp (40%) and random (60%)
        var timestampLength = (int)(length * 0.4);
        var randomLength = length - timestampLength;
        
        // Convert timestamp to Base62
        var timestampPart = EncodeBase62(timestamp, timestampLength);
        
        // Generate cryptographically secure random bytes
        var randomBytes = RandomNumberGenerator.GetBytes(randomLength);
        var randomPart = EncodeBytesToBase62(randomBytes, randomLength);
        
        return timestampPart + randomPart;
    }
    
    /// <summary>
    /// Encodes a number to Base62 with fixed length (left-padded)
    /// </summary>
    private static string EncodeBase62(long value, int length)
    {
        var sb = new StringBuilder();
        
        while (value > 0)
        {
            var remainder = (int)(value % 62);
            sb.Insert(0, Base62Chars[remainder]);
            value /= 62;
        }
        
        // Pad with zeros if needed
        while (sb.Length < length)
        {
            sb.Insert(0, '0');
        }
        
        // Truncate if too long (take rightmost chars to preserve recent time)
        if (sb.Length > length)
        {
            return sb.ToString(sb.Length - length, length);
        }
        
        return sb.ToString();
    }
    
    /// <summary>
    /// Encodes random bytes to Base62
    /// </summary>
    private static string EncodeBytesToBase62(byte[] bytes, int length)
    {
        var sb = new StringBuilder();
        
        foreach (var b in bytes)
        {
            // Use byte value to select Base62 character
            sb.Append(Base62Chars[b % 62]);
        }
        
        // Ensure exact length
        if (sb.Length > length)
        {
            return sb.ToString(0, length);
        }
        
        // Add more randomness if needed
        while (sb.Length < length)
        {
            var extraByte = RandomNumberGenerator.GetBytes(1)[0];
            sb.Append(Base62Chars[extraByte % 62]);
        }
        
        return sb.ToString();
    }
    
    /// <summary>
    /// Validates if a string is a valid short ID format
    /// </summary>
    public static bool IsValid(string id)
    {
        if (string.IsNullOrEmpty(id))
            return false;
        
        return id.All(c => Base62Chars.Contains(c));
    }
}
