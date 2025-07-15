namespace PurchaseFxConverter.Tests.Infra.Services;

public class MockCurrencyConversionServiceTests
{
    [TestCase("EUR", ExpectedResult = 0.92)]
    [TestCase("BRL", ExpectedResult = 5.32)]
    public async Task<decimal?> Should_Return_Mocked_Rate(string currency)
    {
        var service = new MockCurrencyConversionService();
        return await service.GetExchangeRateAsync(currency, DateTime.UtcNow);
    }

    [Test]
    public async Task Should_Return_Null_For_Unsupported_Currency()
    {
        var service = new MockCurrencyConversionService();
        var rate = await service.GetExchangeRateAsync("XYZ", DateTime.UtcNow);

        Assert.That(rate, Is.Null);
    }
}