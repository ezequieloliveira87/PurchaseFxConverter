namespace PurchaseFxConverter.Infra.Services;

public class MockCurrencyConversionService : ICurrencyConversionService
{
    private readonly Dictionary<string, decimal> _mockRates = new()
    {
        { "EUR", 0.92m },
        { "GBP", 0.78m },
        { "BRL", 5.32m },
        { "JPY", 140.15m }
    };

    public Task<decimal?> GetExchangeRateAsync(string targetCurrencyCode, DateTime referenceDate)
    {
        if (_mockRates.TryGetValue(targetCurrencyCode.ToUpper(), out var rate))
            return Task.FromResult<decimal?>(rate);

        return Task.FromResult<decimal?>(null);
    }
}