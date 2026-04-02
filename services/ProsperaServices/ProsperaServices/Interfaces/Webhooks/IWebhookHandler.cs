namespace ProsperaServices.Interfaces.Webhooks;

public interface IWebhookHandler
{
    string Name { get; }
    string Methods => HttpMethods.Get;
    Task Handle(HttpContext context);
}