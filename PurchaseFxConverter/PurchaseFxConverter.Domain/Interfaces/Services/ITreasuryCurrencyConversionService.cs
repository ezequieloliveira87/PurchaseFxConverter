namespace PurchaseFxConverter.Domain.Interfaces.Services;

public interface ITreasuryCurrencyConversionService
{
    Task<decimal?> GetExchangeRateAsync(string targetCurrencyCode, DateTime referenceDate);
}