using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using EvoPayment.Hepers;
using EvoPayment.Implementations;
using EvoPaymentSDK.Models;
using EvoPaymentSDK.Models.Inputs;
using EvoPaymentSDK.Models.Responses;

namespace EvoPaymentSDK.Implementations;

public class SessionRequestNoParameters
{
    private readonly HttpClient _client;

    internal SessionRequestNoParameters(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public Task<EvoResponse<SessionResponse>> Create(CancellationToken cancellationToken = default)
    {
        return Create(null, cancellationToken);
    }

    public async Task<EvoResponse<SessionResponse>> Create(CreateSessionInput? input, CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilderHelper($"session");
        
        var response = await _client.PostAsync(uriBuilder.ToString(), JsonContent.Create((object?)input ?? new {}, options: new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        }), cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            return (await response.Content.ReadFromJsonAsync<EvoError>(cancellationToken: cancellationToken))!;
        }
        return (await response.Content.ReadFromJsonAsync<SessionResponse>(cancellationToken: cancellationToken))!;
    }

    public async Task<EvoResponse<SessionResponse>> Update(UpdateSessionInput input, CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilderHelper($"session");
        
        var response = await _client.PostAsync(uriBuilder.ToString(), JsonContent.Create((object?)input ?? new {}, options: new JsonSerializerOptions
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