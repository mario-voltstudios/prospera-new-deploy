using ProsperaServices.Modes;

namespace ProsperaServices.Interfaces.Payment;

public interface IPaymentGateway;

public interface IPaymentGateway<in T> : IPaymentGateway
    where T : IPaymentInput
{
    Task<Result<ChargeResponse>>  ChargeAsync(T input, CancellationToken token = default);
}