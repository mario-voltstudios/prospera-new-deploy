using System.Text.Json;
using System.Text.Json.Serialization;
using ProsperaServices.Constants;
using ProsperaServices.Interfaces;
using ProsperaServices.Modes;
using ProsperaServices.Utilities;
using ProsperaServices.Modes.Errors.BaseError;

namespace ProsperaServices.Models;

public class SessionInfo
{
    private const int ExpiresInMinutes = 20;
    
    private static readonly JsonSerializerOptions CompactJsonOptions = new()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };
    
    [JsonPropertyName("id")]
    public string SessionId { get; init; }
    
    /// <summary>
    /// Unix timestamp (seconds since epoch) - much smaller than ISO 8601 date string
    /// Example: 1738238400 (10 chars) vs "2026-01-30T10:30:00Z" (20+ chars)
    /// </summary>
    [JsonPropertyName("c")]
    public long CreatedAtUnixSeconds { get; init; }
    
    [JsonConstructor]
    private SessionInfo()
    {
       
    }

    public static SessionInfo CreateSessionInfo()
    {
        return new SessionInfo
        {
            SessionId = ShortIdGenerator.Generate(IdConfiguration.SessionIdLength),
            CreatedAtUnixSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };
    }
    
    [JsonIgnore]
    public DateTime CreationTime => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixSeconds).UtcDateTime;
    
    public TimeSpan TimeToExpireInMinutes => TimeSpan.FromMinutes(ExpiresInMinutes) - (DateTime.UtcNow - CreationTime);
    
    internal static bool GetSessionInfo(IEncryptionService encryptionService, string sessionToken, out Result<SessionInfo> result)
    {
        if (!encryptionService.TryDecrypt(sessionToken, out var sessionString) || string.IsNullOrEmpty(sessionString))
        {
            result = new Error
            {
                Title = "Invalid Session Token",
                Description = "The provided session token is invalid.",
            };
            return true;
        }

        var sessionInfo = JsonSerializer.Deserialize<SessionInfo>(sessionString, CompactJsonOptions);

        if (sessionInfo is null)
        {
            result = new Error
            {
                Title = "Invalid Session Token",
                Description = "The provided session token is invalid.",
            };
            return true;
        }
        
        if(sessionInfo.CreationTime < DateTime.UtcNow.AddMinutes(-ExpiresInMinutes))
        {
            result = new Error
            {
                Title = "Session Expired",
                Description = "The session has expired. Please create a new session.",
            };
            return true;
        }

        result =  sessionInfo;
        return false;
    }
}