using System.Net.Http.Json;
using EvoPayment.Models.Inputs;

namespace EvoPayment.Implementations;

public class TransactionRequestNoParams
{
    private readonly HttpClient _client;

    public TransactionRequestNoParams(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }
    
    public Task BalanceInquiry(CancellationToken cancellationToken = default)
    {
        return BalanceInquiry(null, cancellationToken);
    }
    
    public Task BalanceInquiry(BalanceInquiryInput? input, CancellationToken cancellationToken = default)
    {
       throw  new NotImplementedException();
    }
    
    public Task UpdateApplicationTransactionCounter(CancellationToken cancellationToken = default)
    {
        return UpdateApplicationTransactionCounter(null, cancellationToken);
    }
    
    public Task UpdateApplicationTransactionCounter(UpdateApplicationTransactionCounterInput input,CancellationToken cancellationToken = default)
    {
        throw  new NotImplementedException();
    }
    
}