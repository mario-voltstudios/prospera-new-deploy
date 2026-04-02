using ProsperaServices.Interfaces.Webhooks;

namespace ProsperaServices.Services;

public static class WebhookServices
{
    extension (IServiceCollection services){
        public IServiceCollection AddWebhooks()
        {
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(WebhookServices))
                .AddClasses(classes => classes.AssignableTo<IWebhookHandler>())
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            return services;
        }
    }
}