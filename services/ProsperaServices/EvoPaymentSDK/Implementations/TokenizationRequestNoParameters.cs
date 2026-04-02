using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using EvoPayment.Hepers;
using EvoPayment.Implementations;
using EvoPayment.Models.Inputs;
using EvoPaymentSDK.Models;
using EvoPaymentSDK.Models.Inputs;
using EvoPaymentSDK.Models.Responses;

namespace EvoPaymentSDK.Implementations;

public class TokenizationRequestNoParameters
{
    private readonly HttpClient _client;

    internal TokenizationRequestNoParameters(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }
    
    public async Task<EvoResponse<CreateOrUpdateTokenResponse>> CreateOrUpdateToken(CreateOrUpdateTokenInput input,
        CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilderHelper($"token");
        
        var response = await _client.PostAsync(uriBuilder.ToString(), JsonContent.Create((object?)input ?? new {},options: new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        }), cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            return (await response.Content.ReadFromJsonAsync<EvoError>(cancellationToken: cancellationToken))!;
        }
        
        return (await response.Content.ReadFromJsonAsync<CreateOrUpdateTokenResponse>(cancellationToken: cancellationToken))!;
    }
}