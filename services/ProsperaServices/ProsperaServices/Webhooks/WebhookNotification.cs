using System.Text.Json.Serialization;
using EvoPaymentSDK.Enuns;

namespace ProsperaServices.Webhooks;

/// <summary>
/// Deserialization model for Mastercard Gateway webhook notifications.
/// The payload mirrors the structure of a PayResponse from the gateway.
/// </summary>
public class WebhookNotification
{
    [JsonPropertyName("order")]
    public WebhookOrder? Order { get; set; }

    [JsonPropertyName("transaction")]
    public WebhookTransaction? Transaction { get; set; }

    [JsonPropertyName("response")]
    public WebhookResponse? Response { get; set; }

    [JsonPropertyName("merchant")]
    public string? Merchant { get; set; }

    [JsonPropertyName("result")]
    public TransactionResult? Result { get; set; }
}

public class WebhookOrder
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("reference")]
    public string? Reference { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("creationTime")]
    public DateTime? CreationTime { get; set; }

    [JsonPropertyName("lastUpdatedTime")]
    public DateTime? LastUpdatedTime { get; set; }
}

public class WebhookTransaction
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }
}

public class WebhookResponse
{
    [JsonPropertyName("gatewayCode")]
    public GatewayCode? GatewayCode { get; set; }
}
