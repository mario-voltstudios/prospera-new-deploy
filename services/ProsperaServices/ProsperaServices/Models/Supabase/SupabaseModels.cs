using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ProsperaServices.Models.Supabase;

/// <summary>
/// Maps to the payment_transactions table in Supabase.
/// Tracks all payment attempts and their statuses.
/// </summary>
[Table("payment_transactions")]
public class PaymentTransaction : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("order_reference")]
    public string OrderReference { get; set; } = string.Empty;

    [Column("transaction_reference")]
    public string? TransactionReference { get; set; }

    [Column("recibo_number")]
    public string? ReciboNumber { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("currency")]
    public string Currency { get; set; } = "MXN";

    [Column("status")]
    public string Status { get; set; } = "pending";

    [Column("gateway_code")]
    public string? GatewayCode { get; set; }

    [Column("retry_count")]
    public int RetryCount { get; set; }

    [Column("customer_id")]
    public string? CustomerId { get; set; }

    [Column("payment_gateway")]
    public string PaymentGateway { get; set; } = "evopayment";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }

    [Column("token_data")]
    public string? TokenData { get; set; }

    [Column("error_message")]
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Maps to the paid_receipt table in Supabase.
/// Records successful payments for receipts.
/// </summary>
[Table("paid_receipt")]
public class PaidReceipt : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("receipt_number")]
    public string ReceiptNumber { get; set; } = string.Empty;

    [Column("order_reference")]
    public string OrderReference { get; set; } = string.Empty;

    [Column("payment_method")]
    public string PaymentMethod { get; set; } = "evopayment";

    [Column("paid_at")]
    public DateTime PaidAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Maps to the receipts table in Supabase.
/// Existing table with recibo information.
/// </summary>
[Table("receipts")]
public class Receipt : BaseModel
{
    [PrimaryKey("NUMERO_RECIBO", false)]
    [Column("NUMERO_RECIBO")]
    public string NumeroRecibo { get; set; } = string.Empty;

    [Column("ESTATUS_RECIBO")]
    public string? EstatusRecibo { get; set; }

    [Column("IMPORTE_DEL_RECIBO")]
    public decimal? ImporteDelRecibo { get; set; }

    [Column("FECHA_COBRO")]
    public DateTime? FechaCobro { get; set; }

    [Column("POLIZA")]
    public string? Poliza { get; set; }

    [Column("FECHA_PROG_PAGO")]
    public DateTime? FechaProgPago { get; set; }
}
