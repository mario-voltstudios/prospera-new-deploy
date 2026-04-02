using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ProsperaServices.Models.Supabase;

namespace ProsperaServices.Supabase;

/// <summary>
/// Service for interacting with Supabase REST API (PostgREST).
///
/// Connection config:
///   "Supabase:Url"  → https://lszwokdthvgzcjdlwxzp.supabase.co
///   "Supabase:Key"  → service_role key (sb_secret_…)
///
/// The SUPABASE_CONNECTION_STRING env var (postgresql://…) is available
/// for direct Npgsql access if needed in future, but this service uses
/// the REST layer for simplicity and connection-pool safety.
/// </summary>
public class SupabaseService : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SupabaseService> _logger;
    private readonly string _baseUrl;
    private readonly string _apiKey;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public SupabaseService(IConfiguration configuration, ILogger<SupabaseService> logger)
    {
        _logger = logger;
        _baseUrl = configuration["Supabase:Url"]
            ?? throw new InvalidOperationException("Supabase:Url is not configured");
        _apiKey = configuration["Supabase:Key"]
            ?? throw new InvalidOperationException("Supabase:Key is not configured");

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri($"{_baseUrl}/rest/v1/")
        };
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);
        _httpClient.DefaultRequestHeaders.Add("apikey", _apiKey);
    }

    // ─── Solicitudes ────────────────────────────────────────────────

    public async Task<Solicitud?> GetSolicitudByCode(string code, CancellationToken ct = default)
    {
        var url = $"solicitudes?folio=eq.{Uri.EscapeDataString(code.Trim())}&limit=1";
        var results = await GetAsync<List<Solicitud>>(url, ct);
        return results?.FirstOrDefault();
    }

    // ─── Polizas (new table — lowercase columns, FK → solicitudes) ──

    public async Task<Poliza?> GetPolizaBySolicitudId(Guid solicitudId, CancellationToken ct = default)
    {
        var url = $"polizas?solicitud_id=eq.{solicitudId}&limit=1";
        var results = await GetAsync<List<Poliza>>(url, ct);
        return results?.FirstOrDefault();
    }

    // ─── Payment Transactions (uses existing SupabaseModels.cs model) ─

    public async Task<long> CreatePaymentTransaction(PaymentTransaction tx, CancellationToken ct = default)
    {
        // Serialize only the writeable columns (skip id, created_at — let Supabase default them)
        var payload = new
        {
            order_reference = tx.OrderReference,
            transaction_reference = tx.TransactionReference,
            recibo_number = tx.ReciboNumber,
            amount = tx.Amount,
            currency = tx.Currency,
            status = tx.Status,
            gateway_code = tx.GatewayCode,
            customer_id = tx.CustomerId,
            payment_gateway = tx.PaymentGateway,
            token_data = tx.TokenData,
            error_message = tx.ErrorMessage
        };

        var json = JsonSerializer.Serialize(payload, JsonOpts);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        content.Headers.Add("Prefer", "return=representation");

        var request = new HttpRequestMessage(HttpMethod.Post, "payment_transactions")
        {
            Content = content
        };

        var response = await _httpClient.SendAsync(request, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("CreatePaymentTransaction failed: {Status} {Body}", response.StatusCode, body);
            throw new InvalidOperationException($"Supabase insert failed: {response.StatusCode} — {body}");
        }

        var created = JsonSerializer.Deserialize<List<PaymentTransaction>>(body, JsonOpts);
        return created?.FirstOrDefault()?.Id ?? 0;
    }

    public async Task UpdatePaymentTransaction(long id, string status, string? errorMsg = null, CancellationToken ct = default)
    {
        var patch = new Dictionary<string, object?> { ["status"] = status };
        if (errorMsg is not null) patch["error_message"] = errorMsg;
        if (status is "completed" or "failed")
            patch["completed_at"] = DateTime.UtcNow.ToString("O");

        var json = JsonSerializer.Serialize(patch, JsonOpts);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Patch, $"payment_transactions?id=eq.{id}")
        {
            Content = content
        };

        var response = await _httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(ct);
            _logger.LogError("UpdatePaymentTransaction failed: {Status} {Body}", response.StatusCode, body);
        }
    }

    // ─── Recibos ────────────────────────────────────────────────────

    public async Task<List<Recibo>> GetPendingRecibos(int limit = 50, CancellationToken ct = default)
    {
        var url = $"recibos?status=eq.Pendiente&order=created_at.asc&limit={limit}";
        return await GetAsync<List<Recibo>>(url, ct) ?? [];
    }

    // ─── Helpers ────────────────────────────────────────────────────

    private async Task<T?> GetAsync<T>(string relativeUrl, CancellationToken ct)
    {
        var response = await _httpClient.GetAsync(relativeUrl, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Supabase GET {Url} failed: {Status} {Body}", relativeUrl, response.StatusCode, body);
            return default;
        }

        return JsonSerializer.Deserialize<T>(body, JsonOpts);
    }

    public void Dispose() => _httpClient.Dispose();
}
