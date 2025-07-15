namespace PurchaseFxConverter.Infra.Repositories;

public class InMemoryPurchaseTransactionRepository : IPurchaseTransactionRepository
{
    private readonly List<PurchaseTransaction> _transactions = new();

    public Task SaveAsync(PurchaseTransaction transaction)
    {
        _transactions.Add(transaction);
        return Task.CompletedTask;
    }

    public Task<PurchaseTransaction?> GetByIdAsync(Guid id)
    {
        var transaction = _transactions.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(transaction);
    }
}