using ProsperaServices.Interfaces.Payment;

namespace ProsperaServices.PaymentGateway;

public static class RegisterPaymentGateways
{
    extension (IServiceCollection services){
        public void AddPaymentGateways()
        {
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(RegisterPaymentGateways))
                .AddClasses(classes => classes.AssignableTo<IPaymentGateway>())
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        }
    }
}