using EvoPayment.AspNetCore.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EvoPayment.IoC;

public static class Configure
{
    extension(IServiceCollection services)
    {
        public void AddEvoPaymentSDK(Action<EvoPaymentDiConfig> configure)
        {
            AddEvoPaymentHttp(services);
            
            services.AddSingleton(services =>
            {
                var config = new EvoPaymentDiConfig();
                configure.Invoke(config);

                var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();
                config.HttpClient ??= httpClientFactory.CreateClient("EvoPayment");
                
                return new EvoPaymentSdk(config.HttpClient, config.MerchantId, config.UserName, config.Password);
            });
        }
        public void AddEvoPaymentSDK()
        {
            AddEvoPaymentHttp(services);
            services.AddSingleton(services =>
            {
                var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();
                var configuration = services.GetRequiredKeyedService<IConfiguration>(null);
                var merchantId = configuration["EvoPayment:MerchantId"] ?? configuration["PaymentGateway:EvoPayment:MerchantId"] ?? throw new Exception("Missing merchantId");
                var userName = configuration["EvoPayment:Username"] ?? configuration["PaymentGateway:EvoPayment:Username"] ?? throw new Exception("Missing merchantId");
                var password = configuration["EvoPayment:Password"] ?? configuration["PaymentGateway:EvoPayment:Password"] ?? throw new Exception("Missing api key");
                return new EvoPaymentSdk(httpClientFactory.CreateClient("EvoPayment"), merchantId, userName, password);
            });
        }
    }

    private static void AddEvoPaymentHttp(IServiceCollection services)
    {
        services.AddHttpClient("EvoPayment");
    }
}