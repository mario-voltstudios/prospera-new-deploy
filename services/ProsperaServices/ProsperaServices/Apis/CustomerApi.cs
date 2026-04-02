using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;
using ProsperaServices.ApplicationServices;
using ProsperaServices.Contracts;

namespace ProsperaServices.Apis;

public static class CustomerApi
{
    extension(WebApplication app)
    {
        public void MapCustomerV1Apis()
        {
            var apiV1 = app.MapGroup("api");

            apiV1.MapPost("customer/create", async (
                    [FromBody]CreateCustomerInput input,
                    [FromServices]CreateInsuredService service,
                    CancellationToken cancellationToken = default) 
                => await service.Execute(input, cancellationToken));
        }
    }
}