using System.Text.Json;
using AIProcess.Models;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.PromptTemplates.Liquid;
using NJsonSchema;

namespace AIProcess;

public class ProcessAI(AIServices aiServices)
{

    public async Task<PaycheckResult> ProcessPaycheckFile(string path, CancellationToken cancellationToken)
    {
        var provider = new FileExtensionContentTypeProvider();
        
        if (!provider.TryGetContentType(path, out var contentType))
        {
            throw new Exception($"Could not determine content type for file: {path}");
        }
        
        var dataUri = await ConvertFileToBase64Binary(path, contentType, cancellationToken);
        return await ProcessPaycheckFile(dataUri, contentType, cancellationToken);
    }
    public async Task<PaycheckResult> ProcessPaycheckFile(Stream stream, string contentType, CancellationToken cancellationToken)
    {
        var base46String = await ConvertFileToBase64Binary(stream, contentType, cancellationToken);
        return await ProcessPaycheckFile(base46String, contentType, cancellationToken);
    }
    private async Task<PaycheckResult> ProcessPaycheckFile(string base46String, string contentType, CancellationToken cancellationToken)
    {
        var jsonSchema = JsonSchema.FromType<PaycheckResult>();
        
        var kernel = aiServices.CreateKernel(); 
        
        var templateFactory = new LiquidPromptTemplateFactory();
        var resourceStream = typeof(ProcessAI).Assembly.GetManifestResourceStream("AIProcess.PromptTemplates.ExtractInformationPaycheck.yml");
        
        using var liquidPromptYaml = new StreamReader(resourceStream!);
        var function = kernel.CreateFunctionFromPromptYaml(await liquidPromptYaml.ReadToEndAsync(cancellationToken), templateFactory);
        
        var argument = new KernelArguments(new OpenAIPromptExecutionSettings()
        {
            Temperature = 0.2,
            ReasoningEffort = "low",
        })
        {
            { "json_structure", jsonSchema.ToJson() },
            { "binary_content", base46String },
            { "type_of_file", contentType.StartsWith("image") ? "image" : "file"  }
        };
        
        var result = await kernel.InvokeAsync(function, argument, cancellationToken: cancellationToken);
        
        var data = result.ToString();
        
        try
        {
            return PaycheckResult.FromJson(data);
        }
        catch (JsonException e)
        {
            var fix = await TryFixJson(data, e.Message, cancellationToken);
            return PaycheckResult.FromJson(fix);
        }
    }
   
    
    public async Task<IdResult> ProcessIdFile(string path, CancellationToken cancellationToken)
    {
        var provider = new FileExtensionContentTypeProvider();
        
        if (!provider.TryGetContentType(path, out var contentType))
        {
            throw new Exception($"Could not determine content type for file: {path}");
        }
        
        var dataUri = await ConvertFileToBase64Binary(path, contentType, cancellationToken);
        return await ProcessIdFile(dataUri, contentType, cancellationToken);
    }
    public async Task<IdResult> ProcessIdFile(Stream stream, string contentType,CancellationToken cancellationToken)
    {
        var base46String = await ConvertFileToBase64Binary(stream, contentType, cancellationToken);
        return await ProcessIdFile(base46String, contentType, cancellationToken);
    }
    private async Task<IdResult> ProcessIdFile(string base46String, string contentType, CancellationToken cancellationToken)
    {
        var jsonSchema = JsonSchema.FromType<IdResult>();
        
        var kernel = aiServices.CreateKernel(); 
        
        var templateFactory = new LiquidPromptTemplateFactory();
        var resourceStream = typeof(ProcessAI).Assembly.GetManifestResourceStream("AIProcess.PromptTemplates.ExtractIdInformationPrompt.yml");
        
        using var liquidPromptYaml = new StreamReader(resourceStream!);
        var function = kernel.CreateFunctionFromPromptYaml(await liquidPromptYaml.ReadToEndAsync(cancellationToken), templateFactory);
        
        var argument = new KernelArguments(new OpenAIPromptExecutionSettings()
        {
            Temperature = 0.2,
            ReasoningEffort = "low",
        })
        {
            { "json_structure", jsonSchema.ToJson() },
            { "binary_content", base46String },
            { "type_of_file", contentType.StartsWith("image") ? "image" : "file"  }
        };
        
        
        var result = await kernel.InvokeAsync(function, argument, cancellationToken: cancellationToken);
        
        var data = result.ToString();
        try
        {
            return IdResult.FromJson(data);
        }
        catch (JsonException e)
        {
            var fix = await TryFixJson(data, e.Message, cancellationToken);
            return IdResult.FromJson(fix);
        }
    }
    
    
    public async Task<IdResultBack> ProcessIdFileBck(string path, CancellationToken cancellationToken)
    {
        var provider = new FileExtensionContentTypeProvider();
        
        if (!provider.TryGetContentType(path, out var contentType))
        {
            throw new Exception($"Could not determine content type for file: {path}");
        }
        
        var dataUri = await ConvertFileToBase64Binary(path, contentType, cancellationToken);
        return await ProcessIdFileBck(dataUri, contentType, cancellationToken);
    }
    public async Task<IdResultBack> ProcessIdFileBck(Stream stream, string contentType, CancellationToken cancellationToken)
    {
        var base46String = await ConvertFileToBase64Binary(stream, contentType, cancellationToken);
        return await ProcessIdFileBck(base46String, contentType, cancellationToken);
    }
    private async Task<IdResultBack> ProcessIdFileBck(string base64String, string contentType, CancellationToken cancellationToken)
    {
        var jsonSchema = JsonSchema.FromType<IdResultBack>();
        
        var kernel = aiServices.CreateKernel(); 
        
        var templateFactory = new LiquidPromptTemplateFactory();
        var resourceStream = typeof(ProcessAI).Assembly.GetManifestResourceStream("AIProcess.PromptTemplates.ExtractIdInformationBackPrompt.yml");
        
        using var liquidPromptYaml = new StreamReader(resourceStream!);
        var function = kernel.CreateFunctionFromPromptYaml(await liquidPromptYaml.ReadToEndAsync(cancellationToken), templateFactory);
        
        var argument = new KernelArguments(new OpenAIPromptExecutionSettings()
        {
            Temperature = 0.2,
            ReasoningEffort = "low",
        })
        {
            { "json_structure", jsonSchema.ToJson() },
            { "binary_content", base64String },
            { "type_of_file", contentType.StartsWith("image") ? "image" : "file"  }
        };
        
        var result = await kernel.InvokeAsync(function, argument, cancellationToken: cancellationToken);
        
        var data = result.ToString();
        
        try
        {
            return IdResultBack.FromJson(data);
        }
        catch (JsonException e)
        {
            var fix = await TryFixJson(data, e.Message, cancellationToken);
            return IdResultBack.FromJson(fix);
        }
    }


    public async Task<LetterInformationResult> ProcessInstructionLetter(string path, CancellationToken cancellationToken)
    {
        var provider = new FileExtensionContentTypeProvider();
        
        if (!provider.TryGetContentType(path, out var contentType))
        {
            throw new Exception($"Could not determine content type for file: {path}");
        }
        
        var dataUri = await ConvertFileToBase64Binary(path, contentType, cancellationToken);
        return await ProcessInstructionLetter(dataUri, contentType, cancellationToken);
    }
    public async Task<LetterInformationResult> ProcessInstructionLetter(Stream stream, string contentType,
        CancellationToken cancellationToken)
    {
        var base46String = await ConvertFileToBase64Binary(stream, contentType, cancellationToken);
        return await ProcessInstructionLetter(base46String, contentType, cancellationToken);
    }
    private async Task<LetterInformationResult> ProcessInstructionLetter(string base64String, string contentType,
        CancellationToken cancellationToken)
    {
        var jsonSchema = JsonSchema.FromType<LetterInformationResult>();
        
        var kernel = aiServices.CreateKernel(); 
        
        var templateFactory = new LiquidPromptTemplateFactory();
        var resourceStream = typeof(ProcessAI).Assembly.GetManifestResourceStream("AIProcess.PromptTemplates.ExtractInformationFromCartaPrompt.yml");
        
        using var liquidPromptYaml = new StreamReader(resourceStream!);
        var function = kernel.CreateFunctionFromPromptYaml(await liquidPromptYaml.ReadToEndAsync(cancellationToken), templateFactory);
        
        var argument = new KernelArguments(new OpenAIPromptExecutionSettings()
        {
            Temperature = 0.2,
            ReasoningEffort = "low",
        })
        {
            { "json_structure", jsonSchema.ToJson() },
            { "binary_content", base64String },
            { "type_of_file", contentType.StartsWith("image") ? "image" : "file"  }
        };
        
        var result = await kernel.InvokeAsync(function, argument, cancellationToken: cancellationToken);

        var data = result.ToString();
        
        try
        {
            return LetterInformationResult.FromJson(data);
        }
        catch (JsonException e)
        {
            var fix = await TryFixJson(data, e.Message, cancellationToken);
            return LetterInformationResult.FromJson(fix);
        }
    }

    public async Task<PoliceResult> ProcessPoliceFile(string path, CancellationToken cancellationToken)
    {
        var provider = new FileExtensionContentTypeProvider();
        
        if (!provider.TryGetContentType(path, out var contentType))
        {
            throw new Exception($"Could not determine content type for file: {path}");
        }
        
        var dataUri = await ConvertFileToBase64Binary(path, contentType, cancellationToken);
        return await ProcessPoliceFile(dataUri, contentType, cancellationToken);
    }
    public async Task<PoliceResult> ProcessPoliceFile(Stream stream, string contentType, CancellationToken cancellationToken)
    {
        var base46String = await ConvertFileToBase64Binary(stream, contentType, cancellationToken);
        return await ProcessPoliceFile(base46String, contentType, cancellationToken);
    }
    private async Task<PoliceResult> ProcessPoliceFile(string base64String, string contentType,
        CancellationToken cancellationToken)
    {
        var jsonSchema = JsonSchema.FromType<PoliceResult>();
        
        var kernel = aiServices.CreateKernel(); 
        
        var templateFactory = new LiquidPromptTemplateFactory();
        var resourceStream = typeof(ProcessAI).Assembly.GetManifestResourceStream("AIProcess.PromptTemplates.ExtractPolicyInformationPrompt.yml");
        
        using var liquidPromptYaml = new StreamReader(resourceStream!);
        var function = kernel.CreateFunctionFromPromptYaml(await liquidPromptYaml.ReadToEndAsync(cancellationToken), templateFactory);
        
        var argument = new KernelArguments(new OpenAIPromptExecutionSettings()
        {
            Temperature = 0.2,
            ReasoningEffort = "low",
        })
        {
            { "json_structure", jsonSchema.ToJson() },
            { "binary_content", base64String },
            { "type_of_file", contentType.StartsWith("image") ? "image" : "file"  }
        };
        
        var result = await kernel.InvokeAsync(function, argument, cancellationToken: cancellationToken);

        var data = result.ToString();
        
        try
        {
            return PoliceResult.FromJson(data);
        }
        catch (JsonException e)
        {
            var fix = await TryFixJson(data, e.Message, cancellationToken);
            return PoliceResult.FromJson(fix);
        }
    }

    public async Task<List<LocationsPosition>> SearchLocation(string query, CancellationToken cancellationToken)
    {
        var collection =  await GetLocationCollection();
        var results =  collection.SearchAsync(query, top:3, cancellationToken: cancellationToken);

        var resultsList = new List<LocationsPosition>();
        await foreach (var match in results)
        {
            resultsList.Add(match.Record);
        }

        return resultsList;
    }

    private static async Task<string> ConvertFileToBase64Binary(string path, string contentType, CancellationToken cancellationToken)
    {
        var bytesAsync = await File.ReadAllBytesAsync(path, cancellationToken);
        var base64String = Convert.ToBase64String(bytesAsync);
        var dataUri = $"data:{contentType};base64,{base64String}";
        return dataUri;
    }
    private static async Task<string> ConvertFileToBase64Binary(Stream stream, string contentType, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        var bytesAsync = memoryStream.ToArray();
        var base64String = Convert.ToBase64String(bytesAsync);
        var dataUri = $"data:{contentType};base64,{base64String}";
        return dataUri;
    }
    private async Task<string> TryFixJson(string json, string error, CancellationToken cancellationToken = default)
    {
        var kernel = aiServices.CreateKernel();
        var templateFactory = new LiquidPromptTemplateFactory();
        var resourceStream = typeof(ProcessAI).Assembly.GetManifestResourceStream("AIProcess.PromptTemplates.FixJsonTemplate.yml");
        using var liquidPromptYaml = new StreamReader(resourceStream!);
        var function = kernel.CreateFunctionFromPromptYaml(await liquidPromptYaml.ReadToEndAsync(cancellationToken), templateFactory);
        
        var argument = new KernelArguments(new OpenAIPromptExecutionSettings()
        {
            Temperature = 0.2,
            ReasoningEffort = "low",
        })
        {
            { "json_with_problem", json },
            { "error_message", error },
        };
        
        var result = await kernel.InvokeAsync(function, argument, cancellationToken: cancellationToken);
        
        return result.ToString();
    }
    
    private async Task<VectorStoreCollection<string, LocationsPosition>> GetLocationCollection()
    {
        try
        {
            return await GetLocationCollectionInternal();
        }
        catch (VectorStoreException ex) when (ex.Message.Contains("Dimension mismatch"))
        {
            // Delete the old database with wrong dimensions and recreate
            return await GetLocationCollectionInternal();
        }
    }
    
    private async Task<VectorStoreCollection<string, LocationsPosition>> GetLocationCollectionInternal()
    {
        var vectorStore = aiServices.GetVectorStore();
        var collection = vectorStore.GetCollection<string, LocationsPosition>("locations");
        
        var collectionExists = await collection.CollectionExistsAsync();
        
        // Only create and populate if collection doesn't exist
        if (!collectionExists)
        {
            await collection.EnsureCollectionExistsAsync();

            var records = JsonSerializer.Deserialize<List<LocationsPosition>>(""" 
                                                          [
                                                            {
                                                              "code": "VELATORIO 01",
                                                              "name": "Velatorio 01",
                                                              "description": "Establishment providing comprehensive funeral services for beneficiaries and the general public."
                                                            },
                                                            {
                                                              "code": "UMF C/HOSP 06",
                                                              "name": "Unidad de Medicina Familiar con Hospitalización 06",
                                                              "description": "Primary care unit that also provides basic inpatient hospital services, combining outpatient family medicine with limited hospitalization."
                                                            },
                                                            {
                                                              "code": "UMF 97",
                                                              "name": "Unidad de Medicina Familiar 97",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 96",
                                                              "name": "Unidad de Medicina Familiar 96",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 95",
                                                              "name": "Unidad de Medicina Familiar 95",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 93",
                                                              "name": "Unidad de Medicina Familiar 93",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 92",
                                                              "name": "Unidad de Medicina Familiar 92",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 91",
                                                              "name": "Unidad de Medicina Familiar 91",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 89",
                                                              "name": "Unidad de Medicina Familiar 89",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 88",
                                                              "name": "Unidad de Medicina Familiar 88",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 87",
                                                              "name": "Unidad de Medicina Familiar 87",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 86",
                                                              "name": "Unidad de Medicina Familiar 86",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 85",
                                                              "name": "Unidad de Medicina Familiar 85",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 84",
                                                              "name": "Unidad de Medicina Familiar 84",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 83",
                                                              "name": "Unidad de Medicina Familiar 83",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 82",
                                                              "name": "Unidad de Medicina Familiar 82",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 81",
                                                              "name": "Unidad de Medicina Familiar 81",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 80",
                                                              "name": "Unidad de Medicina Familiar 80",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 79",
                                                              "name": "Unidad de Medicina Familiar 79",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 78",
                                                              "name": "Unidad de Medicina Familiar 78",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 77",
                                                              "name": "Unidad de Medicina Familiar 77",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 75",
                                                              "name": "Unidad de Medicina Familiar 75",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 74",
                                                              "name": "Unidad de Medicina Familiar 74",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 73",
                                                              "name": "Unidad de Medicina Familiar 73",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 70",
                                                              "name": "Unidad de Medicina Familiar 70",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 69",
                                                              "name": "Unidad de Medicina Familiar 69",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 68",
                                                              "name": "Unidad de Medicina Familiar 68",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 67",
                                                              "name": "Unidad de Medicina Familiar 67",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 66",
                                                              "name": "Unidad de Medicina Familiar 66",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 65",
                                                              "name": "Unidad de Medicina Familiar 65",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 64",
                                                              "name": "Unidad de Medicina Familiar 64",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 63",
                                                              "name": "Unidad de Medicina Familiar 63",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 62",
                                                              "name": "Unidad de Medicina Familiar 62",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 61",
                                                              "name": "Unidad de Medicina Familiar 61",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 59",
                                                              "name": "Unidad de Medicina Familiar 59",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 58",
                                                              "name": "Unidad de Medicina Familiar 58",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 56",
                                                              "name": "Unidad de Medicina Familiar 56",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 55",
                                                              "name": "Unidad de Medicina Familiar 55",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 54",
                                                              "name": "Unidad de Medicina Familiar 54",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 52",
                                                              "name": "Unidad de Medicina Familiar 52",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 51",
                                                              "name": "Unidad de Medicina Familiar 51",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 44",
                                                              "name": "Unidad de Medicina Familiar 44",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 43",
                                                              "name": "Unidad de Medicina Familiar 43",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 42",
                                                              "name": "Unidad de Medicina Familiar 42",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 41",
                                                              "name": "Unidad de Medicina Familiar 41",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 40",
                                                              "name": "Unidad de Medicina Familiar 40",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 39",
                                                              "name": "Unidad de Medicina Familiar 39",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 38",
                                                              "name": "Unidad de Medicina Familiar 38",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 37",
                                                              "name": "Unidad de Medicina Familiar 37",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 36",
                                                              "name": "Unidad de Medicina Familiar 36",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 35",
                                                              "name": "Unidad de Medicina Familiar 35",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 34",
                                                              "name": "Unidad de Medicina Familiar 34",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 33",
                                                              "name": "Unidad de Medicina Familiar 33",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 32",
                                                              "name": "Unidad de Medicina Familiar 32",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 250",
                                                              "name": "Unidad de Medicina Familiar 250",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 249",
                                                              "name": "Unidad de Medicina Familiar 249",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 248",
                                                              "name": "Unidad de Medicina Familiar 248",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 247",
                                                              "name": "Unidad de Medicina Familiar 247",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 246",
                                                              "name": "Unidad de Medicina Familiar 246",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 198",
                                                              "name": "Unidad de Medicina Familiar 198",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 195",
                                                              "name": "Unidad de Medicina Familiar 195",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 193",
                                                              "name": "Unidad de Medicina Familiar 193",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 192",
                                                              "name": "Unidad de Medicina Familiar 192",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 191",
                                                              "name": "Unidad de Medicina Familiar 191",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 190",
                                                              "name": "Unidad de Medicina Familiar 190",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 189",
                                                              "name": "Unidad de Medicina Familiar 189",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 188",
                                                              "name": "Unidad de Medicina Familiar 188",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 187",
                                                              "name": "Unidad de Medicina Familiar 187",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 186",
                                                              "name": "Unidad de Medicina Familiar 186",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 185",
                                                              "name": "Unidad de Medicina Familiar 185",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 184",
                                                              "name": "Unidad de Medicina Familiar 184",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 183",
                                                              "name": "Unidad de Medicina Familiar 183",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 182",
                                                              "name": "Unidad de Medicina Familiar 182",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 181",
                                                              "name": "Unidad de Medicina Familiar 181",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 180",
                                                              "name": "Unidad de Medicina Familiar 180",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 12",
                                                              "name": "Unidad de Medicina Familiar 12",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 11",
                                                              "name": "Unidad de Medicina Familiar 11",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 10",
                                                              "name": "Unidad de Medicina Familiar 10",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 09",
                                                              "name": "Unidad de Medicina Familiar 09",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 08",
                                                              "name": "Unidad de Medicina Familiar 08",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 07",
                                                              "name": "Unidad de Medicina Familiar 07",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 05",
                                                              "name": "Unidad de Medicina Familiar 05",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 04",
                                                              "name": "Unidad de Medicina Familiar 04",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 03",
                                                              "name": "Unidad de Medicina Familiar 03",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 02",
                                                              "name": "Unidad de Medicina Familiar 02",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMF 01",
                                                              "name": "Unidad de Medicina Familiar 01",
                                                              "description": "Primary care medical unit providing outpatient family medicine, preventive care, and health promotion."
                                                            },
                                                            {
                                                              "code": "UMAA 01",
                                                              "name": "Unidad Médica de Atención Ambulatoria 01",
                                                              "description": "Specialized medical unit for surgical procedures and treatments not requiring extended hospitalization."
                                                            },
                                                            {
                                                              "code": "U DEPORT 01",
                                                              "name": "Unidad Deportiva 01",
                                                              "description": "Sports facility promoting physical activity and wellness as part of social benefits."
                                                            },
                                                            {
                                                              "code": "TIENDA EMPLEADOS IMSS 03",
                                                              "name": "Tienda de Autoservicio para Empleados IMSS 03",
                                                              "description": "Retail store offering basic goods and supplies for IMSS employees."
                                                            },
                                                            {
                                                              "code": "TIENDA EMPLEADOS IMSS 02 ECATEPEC",
                                                              "name": "Tienda de Autoservicio para Empleados IMSS 02 Ecatepec",
                                                              "description": "Retail store offering basic goods and supplies for IMSS employees in Ecatepec."
                                                            },
                                                            {
                                                              "code": "TIENDA EMPLEADOS IMSS 01",
                                                              "name": "Tienda de Autoservicio para Empleados IMSS 01",
                                                              "description": "Retail store offering basic goods and supplies for IMSS employees."
                                                            },
                                                            {
                                                              "code": "TEATRO 02",
                                                              "name": "Teatro IMSS 02",
                                                              "description": "Cultural venue for performing arts and social events managed by IMSS."
                                                            },
                                                            {
                                                              "code": "TEATRO 01",
                                                              "name": "Teatro IMSS 01",
                                                              "description": "Cultural venue for performing arts and social events managed by IMSS."
                                                            },
                                                            {
                                                              "code": "TALLER PROTESIS Y ORTESIS",
                                                              "name": "Taller de Prótesis y Órtesis",
                                                              "description": "Workshop specialized in the manufacturing and repair of prosthetic and orthotic devices."
                                                            },
                                                            {
                                                              "code": "SUBDELEG 03 LOS REYES LA PASA",
                                                              "name": "Subdelegación 03 Los Reyes La Paz",
                                                              "description": "Administrative office handling affiliation, collection, and beneficiary services in Los Reyes La Paz."
                                                            },
                                                            {
                                                              "code": "SUBDELEG 02 ECATEPEC",
                                                              "name": "Subdelegación 02 Ecatepec",
                                                              "description": "Administrative office handling affiliation, collection, and beneficiary services in Ecatepec."
                                                            },
                                                            {
                                                              "code": "SUBDELEG 02",
                                                              "name": "Subdelegación 02",
                                                              "description": "Administrative office handling affiliation, collection, and beneficiary services."
                                                            },
                                                            {
                                                              "code": "SUBDELEG 01",
                                                              "name": "Subdelegación 01",
                                                              "description": "Administrative office handling affiliation, collection, and beneficiary services."
                                                            },
                                                            {
                                                              "code": "Sin información",
                                                              "name": "Sin Información",
                                                              "description": "No data available for this entry."
                                                            },
                                                            {
                                                              "code": "RESID CONSER PERIF 27",
                                                              "name": "Residencia de Conservación Periférica 27",
                                                              "description": "Maintenance unit responsible for facility and equipment upkeep in the zone."
                                                            },
                                                            {
                                                              "code": "RESID CONSER PERIF 26",
                                                              "name": "Residencia de Conservación Periférica 26",
                                                              "description": "Maintenance unit responsible for facility and equipment upkeep in the zone."
                                                            },
                                                            {
                                                              "code": "RESID CONSER PERIF 25",
                                                              "name": "Residencia de Conservación Periférica 25",
                                                              "description": "Maintenance unit responsible for facility and equipment upkeep in the zone."
                                                            },
                                                            {
                                                              "code": "RESID CONSER PERIF 24",
                                                              "name": "Residencia de Conservación Periférica 24",
                                                              "description": "Maintenance unit responsible for facility and equipment upkeep in the zone."
                                                            },
                                                            {
                                                              "code": "RESID CONSER PERIF 20",
                                                              "name": "Residencia de Conservación Periférica 20",
                                                              "description": "Maintenance unit responsible for facility and equipment upkeep in the zone."
                                                            },
                                                            {
                                                              "code": "RESID CONSER PERIF 19",
                                                              "name": "Residencia de Conservación Periférica 19",
                                                              "description": "Maintenance unit responsible for facility and equipment upkeep in the zone."
                                                            },
                                                            {
                                                              "code": "RESID CONSER PERIF 15",
                                                              "name": "Residencia de Conservación Periférica 15",
                                                              "description": "Maintenance unit responsible for facility and equipment upkeep in the zone."
                                                            },
                                                            {
                                                              "code": "RESID CONSER PERIF 14",
                                                              "name": "Residencia de Conservación Periférica 14",
                                                              "description": "Maintenance unit responsible for facility and equipment upkeep in the zone."
                                                            },
                                                            {
                                                              "code": "RESID CONSER PERIF 10",
                                                              "name": "Residencia de Conservación Periférica 10",
                                                              "description": "Maintenance unit responsible for facility and equipment upkeep in the zone."
                                                            },
                                                            {
                                                              "code": "RESID CONSER PERIF 03",
                                                              "name": "Residencia de Conservación Periférica 03",
                                                              "description": "Maintenance unit responsible for facility and equipment upkeep in the zone."
                                                            },
                                                            {
                                                              "code": "RESID CONSER PERIF 02",
                                                              "name": "Residencia de Conservación Periférica 02",
                                                              "description": "Maintenance unit responsible for facility and equipment upkeep in the zone."
                                                            },
                                                            {
                                                              "code": "RESID CONSER PERIF 01",
                                                              "name": "Residencia de Conservación Periférica 01",
                                                              "description": "Maintenance unit responsible for facility and equipment upkeep in the zone."
                                                            },
                                                            {
                                                              "code": "PLANTA LAVADO 01",
                                                              "name": "Planta de Lavado 01",
                                                              "description": "Industrial laundry facility for cleaning and disinfecting hospital linens."
                                                            },
                                                            {
                                                              "code": "OFNA AUX NIVEL D 14",
                                                              "name": "Oficina Auxiliar de Nivel Delegacional 14",
                                                              "description": "Auxiliary administrative support office at the delegation level."
                                                            },
                                                            {
                                                              "code": "OFNA AUX NIVEL D 11",
                                                              "name": "Oficina Auxiliar de Nivel Delegacional 11",
                                                              "description": "Auxiliary administrative support office at the delegation level."
                                                            },
                                                            {
                                                              "code": "OFNA AUX NIVEL D 10",
                                                              "name": "Oficina Auxiliar de Nivel Delegacional 10",
                                                              "description": "Auxiliary administrative support office at the delegation level."
                                                            },
                                                            {
                                                              "code": "OFNA AUX NIVEL D 09",
                                                              "name": "Oficina Auxiliar de Nivel Delegacional 09",
                                                              "description": "Auxiliary administrative support office at the delegation level."
                                                            },
                                                            {
                                                              "code": "OFNA AUX NIVEL D 08",
                                                              "name": "Oficina Auxiliar de Nivel Delegacional 08",
                                                              "description": "Auxiliary administrative support office at the delegation level."
                                                            },
                                                            {
                                                              "code": "OFNA AUX NIVEL D 05",
                                                              "name": "Oficina Auxiliar de Nivel Delegacional 05",
                                                              "description": "Auxiliary administrative support office at the delegation level."
                                                            },
                                                            {
                                                              "code": "OFNA AUX NIVEL D 04",
                                                              "name": "Oficina Auxiliar de Nivel Delegacional 04",
                                                              "description": "Auxiliary administrative support office at the delegation level."
                                                            },
                                                            {
                                                              "code": "OFNA AUX NIVEL D 03",
                                                              "name": "Oficina Auxiliar de Nivel Delegacional 03",
                                                              "description": "Auxiliary administrative support office at the delegation level."
                                                            },
                                                            {
                                                              "code": "OFNA AUX NIVEL D 02",
                                                              "name": "Oficina Auxiliar de Nivel Delegacional 02",
                                                              "description": "Auxiliary administrative support office at the delegation level."
                                                            },
                                                            {
                                                              "code": "OFNA AUX NIVEL D 01",
                                                              "name": "Oficina Auxiliar de Nivel Delegacional 01",
                                                              "description": "Auxiliary administrative support office at the delegation level."
                                                            },
                                                            {
                                                              "code": "OFNA ALTERNA DELEG",
                                                              "name": "Oficina Alterna de la Delegación",
                                                              "description": "Alternate delegation office for administrative and operational support."
                                                            },
                                                            {
                                                              "code": "HOSPITAL 68",
                                                              "name": "Hospital General 68",
                                                              "description": "General hospital providing secondary level medical care, emergencies, and hospitalization."
                                                            },
                                                            {
                                                              "code": "HOSPITAL 53 LOS REYES LA PAZ",
                                                              "name": "Hospital General 53 Los Reyes La Paz",
                                                              "description": "General hospital providing secondary level medical care in Los Reyes La Paz."
                                                            },
                                                            {
                                                              "code": "HOSPITAL 197 Texcoco",
                                                              "name": "Hospital General 197 Texcoco",
                                                              "description": "General hospital providing secondary level medical care in Texcoco."
                                                            },
                                                            {
                                                              "code": "HOSP TRAUMA Y ORTOPEDIA 01",
                                                              "name": "Hospital de Traumatología y Ortopedia 01",
                                                              "description": "Specialized hospital for the treatment of severe musculoskeletal injuries and trauma."
                                                            },
                                                            {
                                                              "code": "HOSP GRAL Z C/MF 76",
                                                              "name": "Hospital General de Zona con Medicina Familiar 76",
                                                              "description": "General Zone Hospital integrated with a Family Medicine Unit, providing both primary and secondary care."
                                                            },
                                                            {
                                                              "code": "HOSP GRAL Z C/MF 71",
                                                              "name": "Hospital General de Zona con Medicina Familiar 71",
                                                              "description": "General Zone Hospital integrated with a Family Medicine Unit, providing both primary and secondary care."
                                                            },
                                                            {
                                                              "code": "HOSP GRAL Z 98",
                                                              "name": "Hospital General de Zona 98",
                                                              "description": "General Zone Hospital providing secondary care, including specialties and hospitalization."
                                                            },
                                                            {
                                                              "code": "HOSP GRAL Z 58",
                                                              "name": "Hospital General de Zona 58",
                                                              "description": "General Zone Hospital providing secondary care, including specialties and hospitalization."
                                                            },
                                                            {
                                                              "code": "HOSP GRAL Z 57",
                                                              "name": "Hospital General de Zona 57",
                                                              "description": "General Zone Hospital providing secondary care, including specialties and hospitalization."
                                                            },
                                                            {
                                                              "code": "HOSP GRAL Z 194",
                                                              "name": "Hospital General de Zona 194",
                                                              "description": "General Zone Hospital providing secondary care, including specialties and hospitalization."
                                                            },
                                                            {
                                                              "code": "HOSP GRAL REG 72",
                                                              "name": "Hospital General Regional 72",
                                                              "description": "Regional General Hospital offering a wider range of specialties and higher resolution capacity than zone hospitals."
                                                            },
                                                            {
                                                              "code": "HOSP GRAL REG 200",
                                                              "name": "Hospital General Regional 200",
                                                              "description": "Regional General Hospital offering a wider range of specialties and higher resolution capacity than zone hospitals."
                                                            },
                                                            {
                                                              "code": "HOSP GRAL REG 196",
                                                              "name": "Hospital General Regional 196",
                                                              "description": "Regional General Hospital offering a wider range of specialties and higher resolution capacity than zone hospitals."
                                                            },
                                                            {
                                                              "code": "HOSP GRAL REG 08",
                                                              "name": "Hospital General Regional 08",
                                                              "description": "Regional General Hospital offering a wider range of specialties and higher resolution capacity than zone hospitals."
                                                            },
                                                            {
                                                              "code": "HOSP GINECO OBSTETRICIA C/MF 60",
                                                              "name": "Hospital de Gineco-Obstetricia con Medicina Familiar 60",
                                                              "description": "Specialized hospital for women's reproductive health and obstetrics, integrated with family medicine services."
                                                            },
                                                            {
                                                              "code": "HOSP GINECO OBSTETRCIA 221",
                                                              "name": "Hospital de Gineco-Obstetricia 221",
                                                              "description": "Specialized hospital dedicated to gynecology, obstetrics, and neonatal care."
                                                            },
                                                            {
                                                              "code": "GUARD HIJOS MADRES EMPLEADAS IMSS 01",
                                                              "name": "Guardería para Hijos de Madres Empleadas IMSS 01",
                                                              "description": "Childcare center exclusively for children of IMSS employees."
                                                            },
                                                            {
                                                              "code": "GUARD HIJOS MADRES ASEG 47",
                                                              "name": "Guardería para Hijos de Madres Aseguradas 47",
                                                              "description": "Childcare center providing early education, nutrition, and care for children of insured mothers."
                                                            },
                                                            {
                                                              "code": "GUARD HIJOS MADRES ASEG 45",
                                                              "name": "Guardería para Hijos de Madres Aseguradas 45",
                                                              "description": "Childcare center providing early education, nutrition, and care for children of insured mothers."
                                                            },
                                                            {
                                                              "code": "GUARD HIJOS MADRES ASEG 44",
                                                              "name": "Guardería para Hijos de Madres Aseguradas 44",
                                                              "description": "Childcare center providing early education, nutrition, and care for children of insured mothers."
                                                            },
                                                            {
                                                              "code": "GUARD HIJOS MADRES ASEG 37",
                                                              "name": "Guardería para Hijos de Madres Aseguradas 37",
                                                              "description": "Childcare center providing early education, nutrition, and care for children of insured mothers."
                                                            },
                                                            {
                                                              "code": "GUARD HIJOS MADRES ASEG 36",
                                                              "name": "Guardería para Hijos de Madres Aseguradas 36",
                                                              "description": "Childcare center providing early education, nutrition, and care for children of insured mothers."
                                                            },
                                                            {
                                                              "code": "GUARD HIJOS MADRES ASEG 02",
                                                              "name": "Guardería para Hijos de Madres Aseguradas 02",
                                                              "description": "Childcare center providing early education, nutrition, and care for children of insured mothers."
                                                            },
                                                            {
                                                              "code": "GUARD HIJOS MADRES ASEG 01",
                                                              "name": "Guardería para Hijos de Madres Aseguradas 01",
                                                              "description": "Childcare center providing early education, nutrition, and care for children of insured mothers."
                                                            },
                                                            {
                                                              "code": "DEPTO APOYO TEC 03",
                                                              "name": "Departamento de Apoyo Técnico 03",
                                                              "description": "Department providing technical, logistical, and operational support to medical units."
                                                            },
                                                            {
                                                              "code": "DEPTO APOYO TEC 02",
                                                              "name": "Departamento de Apoyo Técnico 02",
                                                              "description": "Department providing technical, logistical, and operational support to medical units."
                                                            },
                                                            {
                                                              "code": "DEPTO APOYO TEC 01",
                                                              "name": "Departamento de Apoyo Técnico 01",
                                                              "description": "Department providing technical, logistical, and operational support to medical units."
                                                            },
                                                            {
                                                              "code": "DELEG 16",
                                                              "name": "Delegación 16",
                                                              "description": "Regional administrative headquarters overseeing IMSS operations in zone 16."
                                                            },
                                                            {
                                                              "code": "DELEG 15",
                                                              "name": "Delegación 15",
                                                              "description": "Regional administrative headquarters overseeing IMSS operations in zone 15."
                                                            },
                                                            {
                                                              "code": "COMUNICACIONES ELECT 01",
                                                              "name": "Comunicaciones Electrónicas 01",
                                                              "description": "Unit managing telecommunications infrastructure and electronic data networks."
                                                            },
                                                            {
                                                              "code": "C SEG SOCIAL 08",
                                                              "name": "Centro de Seguridad Social 08",
                                                              "description": "Social security center offering culture, sports, and technical training activities."
                                                            },
                                                            {
                                                              "code": "C SEG SOCIAL 07",
                                                              "name": "Centro de Seguridad Social 07",
                                                              "description": "Social security center offering culture, sports, and technical training activities."
                                                            },
                                                            {
                                                              "code": "C SEG SOCIAL 06",
                                                              "name": "Centro de Seguridad Social 06",
                                                              "description": "Social security center offering culture, sports, and technical training activities."
                                                            },
                                                            {
                                                              "code": "C SEG SOCIAL 05",
                                                              "name": "Centro de Seguridad Social 05",
                                                              "description": "Social security center offering culture, sports, and technical training activities."
                                                            },
                                                            {
                                                              "code": "C SEG SOCIAL 04",
                                                              "name": "Centro de Seguridad Social 04",
                                                              "description": "Social security center offering culture, sports, and technical training activities."
                                                            },
                                                            {
                                                              "code": "C SEG SOCIAL 03",
                                                              "name": "Centro de Seguridad Social 03",
                                                              "description": "Social security center offering culture, sports, and technical training activities."
                                                            },
                                                            {
                                                              "code": "C SEG SOCIAL 02",
                                                              "name": "Centro de Seguridad Social 02",
                                                              "description": "Social security center offering culture, sports, and technical training activities."
                                                            },
                                                            {
                                                              "code": "C SEG SOCIAL 01",
                                                              "name": "Centro de Seguridad Social 01",
                                                              "description": "Social security center offering culture, sports, and technical training activities."
                                                            },
                                                            {
                                                              "code": "C REG SEG TRAB CAP PROD 01",
                                                              "name": "Centro Regional de Seguridad en el Trabajo, Capacitación y Productividad 01",
                                                              "description": "Regional center for work safety, technical training, and productivity enhancement."
                                                            },
                                                            {
                                                              "code": "C CAP Y PRODUCTI 02",
                                                              "name": "Centro de Capacitación y Productividad 02",
                                                              "description": "Center dedicated to workforce training and productivity improvement programs."
                                                            },
                                                            {
                                                              "code": "C CAP Y PRODUCTI 01",
                                                              "name": "Centro de Capacitación y Productividad 01",
                                                              "description": "Center dedicated to workforce training and productivity improvement programs."
                                                            },
                                                            {
                                                              "code": "ALMACEN GRAL DELEG 01",
                                                              "name": "Almacén General Delegacional 01",
                                                              "description": "General delegation warehouse for logistics, supply storage, and distribution."
                                                            }
                                                          ]
                                                          """) ?? [];
            await collection.UpsertAsync(records);
        }


        return collection;
    }
    
}