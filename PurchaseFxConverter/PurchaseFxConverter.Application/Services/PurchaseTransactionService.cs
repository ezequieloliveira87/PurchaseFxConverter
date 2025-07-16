namespace PurchaseFxConverter.Application.Services;

public class PurchaseTransactionService(
    IPurchaseTransactionRepository repository,
    ICurrencyConversionService currencyService) : IPurchaseTransactionService
{

    public async Task<Guid> CreateAsync(CreatePurchaseTransactionRequest request)
    {
        var transaction = new PurchaseTransaction(request.Description, request.TransactionDate, request.AmountUSD);

        if (!transaction.IsValid)
        {
            var errors = string.Join("; ", transaction.Notifications.Select(n => n.Message));
            throw new ArgumentException(errors);
        }

        await repository.SaveAsync(transaction);
        return transaction.Id;
    }

    public async Task<PurchaseTransactionViewModel?> GetByIdAsync(Guid id)
    {
        var transaction = await repository.GetByIdAsync(id);
        return transaction?.Adapt<PurchaseTransactionViewModel>();
    }

    public async Task<ConvertedTransactionViewModel> ConvertTransactionAsync(Guid id, string targetCurrencyCode)
    {
        var transaction = await repository.GetByIdAsync(id);
        if (transaction is null)
            throw new InvalidOperationException("Transação não encontrada");

        if (!CurrencyCode.IsValid(targetCurrencyCode))
            throw new ArgumentException("Código de moeda inválido");

        var rate = await currencyService.GetExchangeRateAsync(targetCurrencyCode, transaction.TransactionDate);
        if (rate is null)
            throw new InvalidOperationException("Taxa de câmbio indisponível para essa data");

        var convertedAmount = Math.Round(transaction.AmountUSD * rate.Value, 2);

        return new ConvertedTransactionViewModel
        {
            Id = transaction.Id,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate,
            OriginalAmount = transaction.AmountUSD,
            Currency = targetCurrencyCode.ToUpper(),
            ExchangeRate = rate.Value,
            ConvertedAmount = convertedAmount
        };
    }
}