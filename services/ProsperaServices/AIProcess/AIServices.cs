using System.ClientModel;
using AIProcess.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.SqliteVec;
using OpenAI;


namespace AIProcess;

public class AIServices
{
    private readonly Uri _openRouterEndPoint = new("https://openrouter.ai/api/v1");
    private readonly HttpClient _httpClient;
    private readonly string _aiModel;
    private readonly string _embeddingModel;
    private readonly string _openRouterKey;
    private readonly string _serviceId;
    private readonly ILoggerFactory _loggerFactory;



    public AIServices(HttpClient httpClient, string aiModel, string embeddingModel, string openRouterKey, string serviceId, ILoggerFactory loggerFactory)
    {
        _httpClient = httpClient;
        _aiModel = aiModel;
        _embeddingModel = embeddingModel;
        _openRouterKey = openRouterKey;
        _serviceId = serviceId;
        _loggerFactory = loggerFactory;

        httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "https://service.prospera.mx");
        // httpClient.DefaultRequestHeaders.Add("X-Title", $"{softwareName} - {softwareVersion}");

        httpClient.Timeout = TimeSpan.FromMinutes(15);
    }
    
    public Kernel CreateKernel()
    {
        var builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion(_aiModel, _openRouterEndPoint,_openRouterKey,"prospera", serviceId: _serviceId, httpClient: _httpClient);
        return builder.Build();
    }

    public IEmbeddingGenerator<string, Embedding<float>> GetEmbeddingGenerator()
    {
        return new OpenAIClient(new ApiKeyCredential(_openRouterKey), new OpenAIClientOptions
            {
                Endpoint = _openRouterEndPoint,
                ProjectId = _serviceId,
                OrganizationId =  "Prospera",
                UserAgentApplicationId = "ProsperaAIService",
            })
            .GetEmbeddingClient(_embeddingModel)
            .AsIEmbeddingGenerator();
    }

    public VectorStore GetVectorStore()
    {
        var embeddingGenerator = GetEmbeddingGenerator();
        
        return new SqliteVectorStore("Data Source=ai-memory.db;Mode=ReadWriteCreate;Cache=Shared", new SqliteVectorStoreOptions
        {
            EmbeddingGenerator = embeddingGenerator,
        });
    }

}