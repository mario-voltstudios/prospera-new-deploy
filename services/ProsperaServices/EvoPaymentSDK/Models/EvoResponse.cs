using EvoPayment.Implementations;

namespace EvoPaymentSDK.Models;

public class EvoResponse<TSucess>
{
    public bool IsError => Error is not null;
    public EvoError? Error { get; }
    public  TSucess Data { get; }

    internal EvoResponse(TSucess success)
    {
        Data = success;
    }
    
    private EvoResponse(EvoError error)
    {
        Error = error;
    }
    
    public static implicit operator EvoResponse<TSucess>(TSucess success)
    {
        return new EvoResponse<TSucess>(success);
    }
    public static implicit operator EvoResponse<TSucess>(EvoError error)
    {
        return new EvoResponse<TSucess>(error);
    }
}