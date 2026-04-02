using EvoPayment;
using EvoPaymentSDK.Enuns;
using EvoPaymentSDK.Models;
using EvoPaymentSDK.Models.Inputs;
using EvoPaymentSDK.Models.Responses;
using ProsperaServices.Interfaces.Payment;
using ProsperaServices.Modes;
using ProsperaServices.Modes.Errors.BaseError;

namespace ProsperaServices.PaymentGateway.EvoPayment;

public class EvoPaymentHandler(ILogger<EvoPaymentHandler> logger, EvoPaymentSdk evoPayment) : IPaymentGateway<PaymentInput>
{
    
    public async Task<Result<ChargeResponse>> ChargeAsync(PaymentInput input, CancellationToken cancellationToken = default)
    {

        if (input.token.type == PaymentInputType.SessionId)
        {
            logger.LogInformation("Creating token for with SessionId {SessionId}", input.token.data);
            var tokenResultResult = await CreateToken(input.token.data, cancellationToken);
            if (tokenResultResult.IsError)
            {
                logger.LogError("Error creating token for customer {CustomerId}: {@Error}", input.CustomerId, tokenResultResult.Error);
                return  new Error
                {
                    Title = tokenResultResult.Error!.Error.Cause.ToString(),
                    Description = tokenResultResult.Error.Error.Explanation,
                };
            }
            
            logger.LogInformation("Charging customer {CustomerId} total of {Amount} with sessionId {SessionId}", input.CustomerId, input.Amount, input.token.data);

            var chargeResultWithSessionId = await PayWithSessionId(input.Amount, input.token.data, input.OrderId, input.TransactionId, input.ReferenceId, cancellationToken);

            if (chargeResultWithSessionId.IsError)
            {
                logger.LogError("Error processing charge for customer {CustomerId} : {@Error}", input.CustomerId, chargeResultWithSessionId.Error);
                return  new Error
                {
                    Title = chargeResultWithSessionId.Error!.Error.Cause.ToString(),
                    Description = chargeResultWithSessionId.Error.Error.Explanation,
                };
            }
            
            logger.LogInformation("Session carge sucessuly completed for {CustomerId} on the mout of {Amount}", input.CustomerId, input.Amount);
            
            return new ChargeResponse
            {
                OrderId = chargeResultWithSessionId.Data.OrderResponse.Id,
                TransactionId = chargeResultWithSessionId.Data.TransactionResponse.Id,
                Token = tokenResultResult.Data.Token,
                OrderStatus = chargeResultWithSessionId.Data.OrderResponse.Status,
                TrasactionStatus = chargeResultWithSessionId.Data.Result,
                Reference = chargeResultWithSessionId.Data.OrderResponse.Reference,
                Description = Constants.Constants.PaymentDescription(input.ReferenceId, DateTime.Today.ToShortDateString()),
            };
        }
        
        logger.LogInformation("Charging customer {CustomerId} total of {Amount} with tokenId {SessionId}", input.CustomerId, input.Amount, input.token.data);
        var chargeResultWithToken = await PayWithToken(input.Amount, input.token.data, input.OrderId, input.TransactionId, input.ReferenceId, cancellationToken);
       
        if (chargeResultWithToken.IsError)
        {
            logger.LogError("Error processing charge for customer {CustomerId} : {@Error}", input.CustomerId, chargeResultWithToken.Error);
            return  new Error
            {
                Title = chargeResultWithToken.Error!.Error.Cause.ToString(),
                Description = chargeResultWithToken.Error.Error.Explanation,
            };
        }
        
        logger.LogInformation("Token carge sucessuly completed for {CustomerId} on the mout of {Amount}: Reference {Reference}", input.CustomerId, input.Amount, chargeResultWithToken.Data.OrderResponse.Reference);
        return new ChargeResponse
        {
            OrderId = chargeResultWithToken.Data.OrderResponse.Id,
            TransactionId = chargeResultWithToken.Data.TransactionResponse.Id,
            Token = input.token.data,
            OrderStatus = chargeResultWithToken.Data.OrderResponse.Status,
            TrasactionStatus = chargeResultWithToken.Data.Result,
            Reference = chargeResultWithToken.Data.OrderResponse.Reference,
            Description = Constants.Constants.PaymentDescription(input.ReferenceId, DateTime.Today.ToShortDateString()),
        };
    }
    
    public async Task<EvoResponse<CreateOrUpdateTokenResponse>> CreateToken(string sessionId, CancellationToken cancellationToken = default)
    {
        var result = await evoPayment
            .Tokenization()
            .CreateOrUpdateToken(new CreateOrUpdateTokenInput
            {
               Session = new Session
               {
                   SessionId = sessionId,
               }
            }, cancellationToken);
        return result;
    }

    public Task<EvoResponse<PayResponse>> PayWithSessionId(double amount, string sessionId, string orderId, string transactionId, string referenceId, CancellationToken cancellationToken = default)
    {
        return evoPayment.Transaction(orderId, transactionId).Pay(new PayInput
        {
            Order = new Order
            {
                Amount = amount,
                Currency = "MXN",
                Description = Constants.Constants.PaymentDescription(referenceId, DateTime.Today.ToString("yy-MM-dd")),
                Reference = $"{referenceId}_{DateTime.Today:yy-MM-dd}",
            },
            Session = new Session
            {
                SessionId = sessionId
            }
        }, cancellationToken);
    }

    public Task<EvoResponse<PayResponse>> PayWithToken(double amount, string token, string orderId, string transactionId,
        string referenceId, CancellationToken cancellationToken = default)
    {
        return evoPayment.Transaction(orderId, transactionId).Pay(new PayInput
        {
            Order = new Order
            {
                Amount = amount,
                Currency = "MXN",
                Description = Constants.Constants.PaymentDescription(referenceId, DateTime.Today.ToString("yy-MM-dd")),
                Reference = $"{referenceId}_{DateTime.Today:yy-MM-dd}",
            },
            SourceOfFunds = new SourceOfFunds
            {
                Token = token,
            }
        }, cancellationToken);
    }
}