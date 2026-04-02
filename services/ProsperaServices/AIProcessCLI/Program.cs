using System.Text;
using AIProcess;
using AIProcessCLI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Spectre;
using Spectre.Console;
using Spectre.Console.Cli;

Console.Title = "Process Retired Polices";
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
AnsiConsole.Profile.Capabilities.Unicode = true;
AnsiConsole.Profile.Capabilities.Ansi = true;

var resourceStream = typeof(RunAiWork).Assembly.GetManifestResourceStream("AIProcessCLI.appsettings.json");

Log.Logger = new LoggerConfiguration()
    .WriteTo.Spectre(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
        LogEventLevel.Verbose
        )
    .CreateLogger();

var configuration = new ConfigurationBuilder();
configuration.AddJsonStream(resourceStream!);
configuration.AddUserSecrets(typeof(Program).Assembly);
IConfiguration config = configuration.Build();

var services = new ServiceCollection();
services.AddSingleton(config);
services.AddLogging(b =>
{
    b.AddSerilog();
});
services.AddScoped(applicationServices =>
{
    var config = applicationServices.GetRequiredService<IConfiguration>();
    var httpClientFactory = applicationServices.GetRequiredService<IHttpClientFactory>();
    var logFactory = applicationServices.GetRequiredService<ILoggerFactory>();
    return new AIServices(
        httpClientFactory.CreateClient(
            name: "open-router"),
            aiModel: config["OpenRouter:Model"]!,
            embeddingModel: config["OpenRouter:EmbeddingModel"]!,
            openRouterKey: config["OpenRouter:ApiKey"]!,
            serviceId:"application",
            loggerFactory:logFactory
        );
});

services.AddTransient<ProcessAI>();

services.AddHttpClient("open-router")
    .AddStandardResilienceHandler((options) =>
    {
        options.Retry.MaxRetryAttempts = 2;
        options.AttemptTimeout.Timeout = TimeSpan.FromMinutes(5);
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromMinutes(10);
        options.CircuitBreaker.MinimumThroughput = int.MaxValue;
        options.TotalRequestTimeout.Timeout = TimeSpan.FromMinutes(10);
    });

var registrar = new ServiceCollectionRegistrar(services);
var app = new CommandApp<RunAiWork>(registrar);

app.Configure(config =>
{
#if DEBUG
    config.Settings.PropagateExceptions = false;
    config.SetExceptionHandler((ex, _) =>
    {
        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
        return -1;
    });
#endif
});

await app.RunAsync(args);