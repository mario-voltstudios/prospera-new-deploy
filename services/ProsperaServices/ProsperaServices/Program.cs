using AIProcess;
using EvoPayment.IoC;
using ProsperaServices.Apis;
using ProsperaServices.Converters;
using ProsperaServices.Interfaces;
using ProsperaServices.Modes;
using ProsperaServices.PaymentGateway;
using ProsperaServices.Services;
using ProsperaServices.Supabase;
using ProsperaServices.Webhooks.Extension;
using Serilog;
using SerilogTracing;
using TickerQ.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog((context, services, config) => config
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
    );

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddEvoPaymentSDK();
    builder.Services.AddOpenApi();
    builder.Services.AddPaymentGateways();
    builder.Services.AddWebhooks();
    builder.Services.AddApplicationServices();
    builder.Services.AddSingleton<SupabaseService>();
    builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
    builder.Services.AddHybridCache();
    builder.Services.AddScoped(applicationServices =>
    {
        var config = applicationServices.GetRequiredService<IConfiguration>();
        var httpClientFactory = applicationServices.GetRequiredService<IHttpClientFactory>();
        var logFactory = applicationServices.GetRequiredService<ILoggerFactory>();
        return new AIServices(
            httpClientFactory.CreateClient(name: "open-router"),
            aiModel: config["OpenRouter:Model"]!,
            embeddingModel: config["OpenRouter:EmbbedingModel"]!,
            openRouterKey: config["OpenRouter:ApiKey"]!,
            serviceId: "application",
            loggerFactory: logFactory
        );
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "AllowedOrigins",
            policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    builder.Services.AddScoped<ProcessAI>();

    builder.Services.AddTickerQ(options =>
    {
        options.ConfigureScheduler(scheduler =>
        {
            scheduler.MaxConcurrency = 8;
            scheduler.NodeIdentifier = Environment.MachineName;
        });
    });

    builder.Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.Converters.Add(new ResultConverterFactory());
    });
}


var app = builder.Build();
{
    app.UseSerilogRequestLogging();
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseCors("AllowedOrigins");
    // app.UseAntiforgery();
    app.UseHttpsRedirection();
    app.UseWebhooks();
    app.MapCustomerV1Apis();
    app.MapAgentsV1Apis();

    app.UseTickerQ();

    using var activityListenerConfig = new ActivityListenerConfiguration().TraceToSharedLogger();
}


app.Run();