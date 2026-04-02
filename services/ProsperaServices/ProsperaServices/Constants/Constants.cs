namespace ProsperaServices.Constants;

public static class Constants
{
    public static string PaymentDescription(string policyNumber, string date) =>  $"POLIZA VIDA MAS {policyNumber} Recibo del {date}";
}