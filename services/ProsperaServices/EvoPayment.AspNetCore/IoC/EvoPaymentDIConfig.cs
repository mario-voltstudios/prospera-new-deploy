namespace EvoPayment.AspNetCore.IoC;

public class EvoPaymentDiConfig
{
    public string MerchantId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public HttpClient? HttpClient { get; set; }
}