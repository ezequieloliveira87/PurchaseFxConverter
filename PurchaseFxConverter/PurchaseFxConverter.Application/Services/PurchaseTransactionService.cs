namespace PurchaseFxConverter.Application.Services;

public class PurchaseTransactionService(
    IPurchaseTransactionRepository repository,
    ITreasuryCurrencyConversionService treasuryCurrencyService,
    ILogger<PurchaseTransactionService> logger) : IPurchaseTransactionService
{
    public async Task<Guid> CreateAsync(CreatePurchaseTransactionRequest request)
    {
        var transaction = new PurchaseTransaction(request.Description, request.TransactionDate, request.AmountUsd);

        if (!transaction.IsValid)
        {
            var errors = string.Join("; ", transaction.Notifications.Select(n => n.Message));
            throw new ArgumentException(errors);
        }

        await repository.SaveAsync(transaction);

        logger.LogInformation(SuccessMessage.TransactionCreated.GetEnumDescription());
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
            throw new InvalidOperationException(ErrorMessage.TransactionNotFound.GetEnumDescription());

        var exchangeRate = await treasuryCurrencyService.GetExchangeRateAsync(targetCurrencyCode, transaction.TransactionDate);
        if (exchangeRate is null)
            throw new InvalidOperationException(ErrorMessage.ExchangeRateUnavailable.GetEnumDescription());
        var convertedAmount = Math.Round(transaction.AmountUsd * exchangeRate.Value, 2);

        return new ConvertedTransactionViewModel
        {
            Id = transaction.Id,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate,
            OriginalAmount = transaction.AmountUsd,
            Currency = targetCurrencyCode.ToUpper(),
            ExchangeRate = exchangeRate.Value,
            ConvertedAmount = convertedAmount
        };
    }
}