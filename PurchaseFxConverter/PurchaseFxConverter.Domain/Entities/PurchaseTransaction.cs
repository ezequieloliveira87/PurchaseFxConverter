namespace PurchaseFxConverter.Domain.Entities;

public class PurchaseTransaction : Entity  
{
    public string Description { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public decimal AmountUSD { get; private set; }

    public PurchaseTransaction(string description, DateTime transactionDate, decimal amountUSD)
    {
        AddNotifications(new PurchaseTransactionValidator(description, transactionDate, amountUSD));

        if (!IsValid)
            return;

        Description = description?.Trim();
        TransactionDate = transactionDate;
        AmountUSD = Math.Round(amountUSD, 2);
    }
}