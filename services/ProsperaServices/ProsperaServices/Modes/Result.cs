using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ProsperaServices.Converters;
using ProsperaServices.Modes.Errors.BaseError;

namespace ProsperaServices.Modes;

[JsonConverter(typeof(ResultConverterFactory))]
public class Result<T>
{

    public T? Data { get; init; }
    public Error? Error { get; init; }

    private Result(){}

    private bool IsSuccess => Error is null && Data is not null;
    public bool IsError => Error is not null;
    
    public static implicit operator Result<T>(T data)
    {
        // Check if T is a Result<> type to prevent double-wrapping
        var type = data!.GetType();
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>))
        {
            return new Result<T>
            {
                Error = ((dynamic?)data)?.Error,
                Data = ((dynamic?)data)?.Data,
            };
        }
        
        return new Result<T> { Data = data };
    }

    public static implicit operator Result<T>(Error error) => new() { Error = error };


    public (object response, Type type) ToResponse()
    {
        if (IsSuccess)
        {
            return (Data!, typeof(T));
        }
        
        var problemDetails = new ProblemDetails
        {
            Title = Error!.Title,
            Detail = Error.Description,
            Status = StatusCodes.Status400BadRequest,
        };
        
        return (problemDetails, typeof(ProblemDetails));
    }
    
    public void Deconstruct(out T? data, out Error? error)
    {
        data = Data;
        error = Error;
    }
    
}