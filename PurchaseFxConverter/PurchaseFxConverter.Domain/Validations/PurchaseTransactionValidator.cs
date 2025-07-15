namespace PurchaseFxConverter.Domain.Validations;

public class PurchaseTransactionValidator : Contract<Notification>
{
    public PurchaseTransactionValidator(string description, DateTime transactionDate, decimal amountUsd)
    {
        Requires()
            .IsNotNullOrWhiteSpace(description, "Description", "A descrição é obrigatória")
            .IsLowerOrEqualsThan(description, 50, "Description", "A descrição deve ter no máximo 50 caracteres")
            .IsGreaterThan(amountUsd, 0, "AmountUSD", "O valor da compra deve ser maior que zero")
            .IsLowerOrEqualsThan(transactionDate, DateTime.UtcNow, "TransactionDate", "A data da transação não pode ser no futuro");
    }
}