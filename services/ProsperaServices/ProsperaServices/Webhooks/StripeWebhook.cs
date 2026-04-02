using ProsperaServices.Interfaces.Webhooks;
using Serilog;
using Serilog.Events;
using SerilogTracing;
using ILogger = Serilog.ILogger;

namespace ProsperaServices.Webhooks;

public class StripeWebhook : IWebhookHandler
{
    private readonly ILogger _logger;
    private readonly IDiagnosticContext _diagnosticContext;
    private readonly IConfiguration _configuration;

    public StripeWebhook(ILogger logger, IDiagnosticContext diagnosticContext, IConfiguration configuration)
    {
        _logger = logger;
        _diagnosticContext = diagnosticContext;
        _configuration = configuration;
    }

    public string Name => "stripe";
    public string Methods => HttpMethods.Post;

    public async Task Handle(HttpContext context)
    {
        using var activity = _logger.StartActivity(LogEventLevel.Information, "StripeWebhookHandler");
        // _logger.Information("StripeId {id}", _configuration["PaymentGateway:Stripe:Secret"]);
        _diagnosticContext.Set("Webhook", "Stripe-oko");
        await context.Response.WriteAsync("ok");
    }
}