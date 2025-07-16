

namespace PurchaseFxConverter.Application.Interfaces;

public interface IPurchaseTransactionService
{
    Task<Guid> CreateAsync(CreatePurchaseTransactionRequest request);
    Task<PurchaseTransactionViewModel?> GetByIdAsync(Guid id);
    Task<ConvertedTransactionViewModel> ConvertTransactionAsync(Guid id, string targetCurrencyCode);
}