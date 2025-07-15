namespace PurchaseFxConverter.Application.DTOs;

public class CreatePurchaseTransactionRequest
{
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public decimal AmountUSD { get; set; }
}