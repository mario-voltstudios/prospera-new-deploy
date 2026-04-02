using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;
using ProsperaServices.ApplicationServices;
using ProsperaServices.Contracts;

namespace ProsperaServices.Apis;

public static class AgentsApi
{
    public static void MapAgentsV1Apis(this WebApplication app)
    {
        var apiV1 = app.MapGroup("api/agent")
            .DisableAntiforgery();

        apiV1.MapGet("get-session", ([FromServices] ProcessFilesService service)
            => service.CreateSession());

        apiV1.MapPost("process/paycheck", async (
                [FromForm] ProcessFileInput input,
                [FromServices] ProcessFilesService service,
                [FromServices]HybridCache cache,
                CancellationToken cancellationToken = default)
            => await service.ProcessPaycheckFile(input, cache, cancellationToken));

        apiV1.MapPost("process/id-front", async (
                [FromForm] ProcessFileInput input,
                [FromServices] ProcessFilesService service,
                [FromServices]HybridCache cache,
                CancellationToken cancellationToken = default)
            => await service.ProcessIdFrontFile(input, cache, cancellationToken));

        apiV1.MapPost("process/id-back", async (
                [FromForm] ProcessFileInput input,
                [FromServices] ProcessFilesService service,
                [FromServices]HybridCache cache,
                CancellationToken cancellationToken = default)
            => await service.ProcessIdBackFile(input, cache, cancellationToken));

        apiV1.MapPost("process/letter", async (
                [FromForm] ProcessFileInput input,
                [FromServices] ProcessFilesService service,
                [FromServices]HybridCache cache,
                CancellationToken cancellationToken = default)
            => await service.ProcessLetterFile(input, cache, cancellationToken));

        apiV1.MapPost("process/proof-of-address", async (
                [FromForm] ProcessFileInput input,
                [FromServices] ProcessFilesService service,
                [FromServices]HybridCache cache,
                CancellationToken cancellationToken = default)
            => await service.ProcessAddressProof(input, cache, cancellationToken));

        apiV1.MapPost("upload/photo", async (
                [FromForm] ProcessFileInput input,
                [FromServices] ProcessFilesService service,
                [FromServices]HybridCache cache,
                CancellationToken cancellationToken = default)
            => await service.UploadPhoto(input, cache, cancellationToken));
        
        apiV1.MapGet("search/locations", async (
                [FromQuery] string query,
                [FromServices] ProcessFilesService service,
                CancellationToken cancellationToken = default)
            => await service.SearchLocation(query, cancellationToken));

        apiV1.MapPost("process/policy", async (
                [FromForm] ProcessPolicyInput input,
                [FromServices] ProcessFilesService service,
                [FromServices]HybridCache cache,
                CancellationToken cancellationToken = default)
            => await service.ExecutePoliceProcess(input, cache, cancellationToken));
    }
}