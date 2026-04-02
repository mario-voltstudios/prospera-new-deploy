using System.Security.Cryptography.X509Certificates;
using System.Text;
using EvoPayment.Implementations;
using EvoPaymentSDK.Implementations;

namespace EvoPayment;

public class EvoPaymentSdk
{
    private readonly HttpClient _client;

    public EvoPaymentSdk(HttpClient client, string merchantId, string userName, string password) : this(client, merchantId)
    {
        if (client == null) throw new ArgumentNullException(nameof(client));
        if (merchantId == null) throw new ArgumentNullException(nameof(merchantId));
        if (userName == null) throw new ArgumentNullException(nameof(userName));
        if (password == null) throw new ArgumentNullException(nameof(password));

        // build "merchantId:apiKey" and set Basic auth header
        var credentials = $"{userName}:{password}";
        var bytes = Encoding.UTF8.GetBytes(credentials);
        var base64 = Convert.ToBase64String(bytes);

        if (_client.DefaultRequestHeaders.Contains("Authorization"))
            _client.DefaultRequestHeaders.Remove("Authorization");

        _client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64);
    }
        
    public EvoPaymentSdk(HttpClient client, string merchantId, X509Certificate certificate) : this(client, merchantId)
    {
        var merchantId1 = merchantId;
    }
        
    private EvoPaymentSdk(HttpClient client, string merchantId)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _client.DefaultRequestHeaders.Add("Accept", "application/json");
        _client.BaseAddress = new Uri($"https://evopaymentsmexico.gateway.mastercard.com/api/rest/version/100/merchant/{merchantId}/");
    }
        
    public TransactionRequest Transaction(string orderId, string transactionId) => new(_client, orderId, transactionId);
    public TransactionRequestNoParams Transaction() => new(_client);
        
    public TokenizationRequest Tokenization(string tokenId) => new(_client, tokenId);
    public TokenizationRequestNoParameters Tokenization() => new(_client);
    public SessionRequestNoParameters Session() => new(_client);
    public SessionRequest Session(string sessionId) => new(_client, sessionId);
        
}