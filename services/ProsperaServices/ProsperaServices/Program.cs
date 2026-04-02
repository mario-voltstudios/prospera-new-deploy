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

// Configure Serilog early so we can capture startup crashes
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter())
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting up...");

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
                aiModel: config["OpenRouter:Model"] ?? "placeholder",
                embeddingModel: config["OpenRouter:EmbbedingModel"] ?? "placeholder",
                openRouterKey: config["OpenRouter:ApiKey"] ?? "placeholder",
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

    Log.Information("DI container built, building app...");

    var app = builder.Build();
    {
        app.UseSerilogRequestLogging();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseCors("AllowedOrigins");
        app.UseWebhooks();
        app.MapCustomerV1Apis();
        app.MapAgentsV1Apis();
        app.MapGet("/health", () => "OK");

        app.UseTickerQ();

        using var activityListenerConfig = new ActivityListenerConfiguration().TraceToSharedLogger();
    }

    Log.Information("App configured, starting Kestrel...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed");
}
finally
{
    Log.CloseAndFlush();
}
