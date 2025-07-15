using PurchaseFxConverter.Application.Interfaces;

namespace PurchaseFxConverter.Application.Services;

public class PurchaseTransactionService : IPurchaseTransactionService
{
    private readonly IPurchaseTransactionService _service;

    public PurchaseTransactionService(IPurchaseTransactionService service)
    {
        _service = service;
    }

    public async Task<Guid> CreateAsync(CreatePurchaseTransactionRequest request)
    {
        var transaction = new PurchaseTransactionViewModel(
            request.Description,
            request.TransactionDate,
            request.AmountUSD
        );

        if (!transaction.IsValid)
            throw new ArgumentException("Transação inválida", nameof(request));

        await _service.SaveAsync(transaction);
        return transaction.Id;
    }

    public async Task<PurchaseTransactionViewModel?> GetByIdAsync(Guid id)
    {
        var transaction = await _service.GetByIdAsync(id);
        if (transaction is null) return null;

        return new PurchaseTransactionViewModel
        {
            Id = transaction.Id,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate,
            AmountUSD = transaction.AmountUSD,
            CreatedAt = transaction.CreatedAt
        };
    }
}