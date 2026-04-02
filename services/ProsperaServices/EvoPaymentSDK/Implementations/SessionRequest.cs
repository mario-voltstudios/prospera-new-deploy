using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using EvoPayment.Hepers;
using EvoPayment.Implementations;
using EvoPaymentSDK.Models;
using EvoPaymentSDK.Models.Inputs;
using EvoPaymentSDK.Models.Responses;

namespace EvoPaymentSDK.Implementations;

public class SessionRequest
{
    private readonly HttpClient _client;
    private readonly string _sessionId;

    internal SessionRequest(HttpClient client, string sessionId)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _sessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
    }
    

    public async Task<EvoResponse<SessionResponse>> Update(UpdateSessionInput input, CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilderHelper($"session/{_sessionId}");
        
        var response = await _client.PutAsync(uriBuilder.ToString(), JsonContent.Create((object?)input ?? new {}, options: new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        }), cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            return (await response.Content.ReadFromJsonAsync<EvoError>(cancellationToken: cancellationToken))!;
        }
        return (await response.Content.ReadFromJsonAsync<SessionResponse>(cancellationToken: cancellationToken))!;
    }
}