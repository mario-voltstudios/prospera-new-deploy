using System.Text.Json.Serialization;
using Microsoft.Extensions.VectorData;

namespace AIProcess.Models;

public class LocationsPosition
{
    [JsonPropertyName("code")]
    [VectorStoreKey] 
    public string Code { get; set; } = default!;

    [JsonPropertyName("name")]
    [VectorStoreData]
    public string Name { get; set; } = default!;

    [JsonPropertyName("description")]
    [VectorStoreData]
    public string Description { get; set; } = default!;

    [VectorStoreVector(1536)]
    public string Embedding => $"{Code} is {Name} - {Description}";
}