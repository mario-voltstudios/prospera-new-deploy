using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using EvoPayment.Hepers;
using EvoPayment.Implementations;
using EvoPayment.Models.Inputs;
using EvoPaymentSDK.Models;
using EvoPaymentSDK.Models.Inputs;

namespace EvoPaymentSDK.Implementations;
public class TokenizationRequest
{
    private readonly HttpClient _client;
    private readonly string _tokenId;

    internal TokenizationRequest(HttpClient client, string tokenId)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _tokenId = tokenId ?? throw new ArgumentNullException(nameof(tokenId));
    }

    public Task<EvoResponse<object>> CreateOrUpdateToken(CancellationToken cancellationToken = default)
    {
        return CreateOrUpdateToken(null, cancellationToken);
    }
            
    public async Task<EvoResponse<object>> CreateOrUpdateToken(CreateOrUpdateTokenInput? input, CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilderHelper($"token/{_tokenId}");
        
        var response = await _client.PutAsync(uriBuilder.ToString(), JsonContent.Create((object?)input ?? new {}, options: new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        }), cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            return (await response.Content.ReadFromJsonAsync<EvoError>(cancellationToken: cancellationToken))!;
        }
        return (await response.Content.ReadFromJsonAsync<object>(cancellationToken: cancellationToken))!;
    }

    public Task<EvoResponse<object>> DeleteToken(CancellationToken cancellationToken = default)
    {
        return DeleteToken(null, cancellationToken);
    }
    public async Task<EvoResponse<object>> DeleteToken(DeleteTokenInput? input, CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilder($"token/{_tokenId}");
        // uriBuilder.AddQueryParameter("bbb", "2")
        
        var response = await _client.DeleteAsync(uriBuilder.ToString(), cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            return (await response.Content.ReadFromJsonAsync<EvoError>(cancellationToken: cancellationToken))!;
        }
        
        return _client.DeleteFromJsonAsync<object>(new Uri($"token/{_tokenId}"), cancellationToken: cancellationToken);

    }

    public Task<EvoResponse<object>> GetToken(CancellationToken cancellationToken = default)
    {
        return  GetToken(null, cancellationToken);
    }

    public async Task<EvoResponse<object>> GetToken(GetTokenInput? input, CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilderHelper($"token/{_tokenId}");
        // uriBuilder.AddQueryParameter("bbb", "2");
        var response = await _client.GetAsync(uriBuilder.ToString(), cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return (await response.Content.ReadFromJsonAsync<EvoError>(cancellationToken: cancellationToken))!;
        }
        return (await response.Content.ReadFromJsonAsync<object>(cancellationToken: cancellationToken))!;
    }
}