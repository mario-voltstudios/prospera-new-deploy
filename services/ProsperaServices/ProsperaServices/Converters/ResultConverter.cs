using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ProsperaServices.Modes;

namespace ProsperaServices.Converters;

public class ResultConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType)
            return false;

        return typeToConvert.GetGenericTypeDefinition() == typeof(Result<>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var valueType = typeToConvert.GetGenericArguments()[0];
        
        var converterType = typeof(ResultConverter<>).MakeGenericType(valueType);
        
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public class ResultConverter<T> : JsonConverter<Result<T>>
{

    public ResultConverter()
    {
    }
    
    public override Result<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Result<T>? value, JsonSerializerOptions options)
    {
        var (response, type) = value!.ToResponse();
        JsonSerializer.Serialize(writer, response, type, options);
    }
}