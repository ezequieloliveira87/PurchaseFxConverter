namespace PurchaseFxConverter.Domain.Entities;

public class PurchaseTransaction : Notifiable<Notification>
{
    public PurchaseTransaction(string description, DateTime transactionDate, decimal amountUsd)
    {
        AddNotifications(new PurchaseTransactionValidator(description, transactionDate, amountUsd));

        if (!IsValid)
            return;

        Id = Guid.NewGuid();
        Description = description.Trim();
        TransactionDate = transactionDate;
        AmountUsd = Math.Round(amountUsd, 2);
    }
    public Guid Id { get; protected set; }
    public string Description { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public decimal AmountUsd { get; private set; }
}