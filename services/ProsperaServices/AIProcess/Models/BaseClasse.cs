using System.Text.Json;
using System.Text.RegularExpressions;

namespace AIProcess.Models;

public partial class BaseClasse<T>
{
    private static bool _isOptionsInitialized;
    
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
    };
    
    // Helper: parse JSON into LawSuitInformation
    public static T FromJson(string json)
    {
        var m = JsonExtract().Match(json);
        
        
        
        if (m.Success)
        {
            json = m.Groups[1].Value;
        }

        if (!_isOptionsInitialized)
        {
            // Options.Converters.Add(new DateOnlyJsonConverter());
            _isOptionsInitialized = true;
        }

        try
        {
            var result = JsonSerializer.Deserialize<T>(json, Options);
            // if (result == null)
            //     throw new FailToParseJsonException<T>(new Exception("invalid json"), json);
        
            return result;
        }
        catch (Exception e)
        {
            throw;
            // throw new FailToParseJsonException<T>(e, json);
        }
       
    }

    [GeneratedRegex(@"^\s*```(?:\w+)?\s*\r?\n([\s\S]*?)\r?\n\s*```\s*$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex JsonExtract();
}