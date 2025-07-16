namespace PurchaseFxConverter.Application.ViewModels;

public class ConvertedTransactionViewModel
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public decimal OriginalAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; }
    public decimal ConvertedAmount { get; set; }
}