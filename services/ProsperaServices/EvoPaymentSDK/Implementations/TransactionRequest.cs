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
public class TransactionRequest
{
    private readonly HttpClient _client;
    private  readonly string _orderId;
    private readonly string _transactionId;
    
    
    internal TransactionRequest(HttpClient client, string orderId, string transactionId)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _orderId = orderId ?? throw new ArgumentNullException(nameof(orderId));
        _transactionId = transactionId ?? throw new ArgumentNullException(nameof(transactionId));
    }
    public Task<EvoResponse<AutorizeResponse>> Autorize(CancellationToken cancellationToken = default)
    {
        return Autorize(null, cancellationToken);
    }
    public async Task<EvoResponse<AutorizeResponse>> Autorize(AutorizeInput input, CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilderHelper($"order/{_orderId}/transaction/{_transactionId}");
        
        var response = await _client.PutAsync(uriBuilder.ToString(), JsonContent.Create((object?)input ?? new {}, options: new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        }), cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            return (await response.Content.ReadFromJsonAsync<EvoError>(cancellationToken: cancellationToken))!;
        }
        return (await response.Content.ReadFromJsonAsync<AutorizeResponse>(cancellationToken: cancellationToken))!;
        
    }
    
    public Task<EvoResponse<PayResponse>> Pay(CancellationToken cancellationToken = default)
    {
        return Pay(null, cancellationToken);
    }
    public async Task<EvoResponse<PayResponse>> Pay(PayInput? input,CancellationToken cancellationToken = default)
    {
        var uriBuilder = new UriBuilderHelper($"order/{_orderId}/transaction/{_transactionId}");
        var response = await _client.PutAsync(uriBuilder.ToString(), JsonContent.Create((object?)input ?? new {}, options: new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        }), cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            return (await response.Content.ReadFromJsonAsync<EvoError>(cancellationToken: cancellationToken))!;
        }
        
        return (await response.Content.ReadFromJsonAsync<PayResponse>(cancellationToken: cancellationToken))!;
    }
}