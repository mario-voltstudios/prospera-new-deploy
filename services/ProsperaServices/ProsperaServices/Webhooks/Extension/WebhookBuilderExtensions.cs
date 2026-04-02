using ProsperaServices.Interfaces.Webhooks;

namespace ProsperaServices.Webhooks.Extension;

public static class WebhookBuilderExtensions
{
    extension(WebApplication app)
    {
        public void UseWebhooks()
        {
            var webhooks = app.Services.GetServices<IWebhookHandler>();

            foreach (var webhook in webhooks)
            {
                app.MapMethods($"webhook/{webhook.Name}", [webhook.Methods], (Delegate)webhook.Handle)
                   .WithName($"{webhook.Name}Webhook");
            }
        }
    }
}