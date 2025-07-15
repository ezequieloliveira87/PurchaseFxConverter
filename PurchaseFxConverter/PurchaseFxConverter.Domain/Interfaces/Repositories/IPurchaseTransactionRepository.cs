namespace PurchaseFxConverter.Domain.Interfaces.Repositories;

public interface IPurchaseTransactionRepository
{
    Task SaveAsync(PurchaseTransaction transaction);
    Task<PurchaseTransaction?> GetByIdAsync(Guid id);
}