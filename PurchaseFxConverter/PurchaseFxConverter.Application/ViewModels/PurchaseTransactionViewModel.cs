namespace PurchaseFxConverter.Application.ViewModels;

public class PurchaseTransactionViewModel
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public decimal AmountUSD { get; set; }
}