using Mapster;
using PurchaseFxConverter.Application.ViewModels;

namespace PurchaseFxConverter.Tests.Application.Services;

public class PurchaseTransactionServiceTests
{
    private PurchaseTransactionService _service;

    [SetUp]
    public void Setup()
    {
        var repo = new InMemoryPurchaseTransactionRepository();
        var currencyService = new MockCurrencyConversionService();
        _service = new PurchaseTransactionService(repo, currencyService);
    }

    [Test]
    public async Task Should_Create_Transaction_With_Valid_Data()
    {
        var request = new CreatePurchaseTransactionRequest
        {
            Description = "Compra válida",
            TransactionDate = DateTime.UtcNow,
            AmountUSD = 100m
        };

        var id = await _service.CreateAsync(request);

        Assert.That(id, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void Should_Throw_When_Creating_Invalid_Transaction()
    {
        var request = new CreatePurchaseTransactionRequest
        {
            Description = "", // inválido
            TransactionDate = DateTime.UtcNow.AddDays(1), // no futuro
            AmountUSD = 0
        };

        Assert.That(() => _service.CreateAsync(request),
            Throws.ArgumentException.With.Message.Contain("Transação inválida"));
    }

    [Test]
    public async Task Should_Return_Transaction_When_Found()
    {
        var request = new CreatePurchaseTransactionRequest
        {
            Description = "Compra X",
            TransactionDate = DateTime.UtcNow,
            AmountUSD = 50m
        };

        var id = await _service.CreateAsync(request);
        var result = await _service.GetByIdAsync(id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Description, Is.EqualTo("Compra X"));
    }

    [Test]
    public async Task Should_Return_Null_When_Transaction_Not_Found()
    {
        var result = await _service.GetByIdAsync(Guid.NewGuid());
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task Should_Convert_Transaction_With_Valid_Currency()
    {
        var request = new CreatePurchaseTransactionRequest
        {
            Description = "Compra Y",
            TransactionDate = DateTime.UtcNow,
            AmountUSD = 10m
        };

        var id = await _service.CreateAsync(request);
        var result = await _service.ConvertTransactionAsync(id, "EUR");

        Assert.That(result.ConvertedAmount, Is.GreaterThan(0));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public async Task Should_Throw_When_Currency_Is_Invalid()
    {
        // Arrange: cria uma transação real
        var request = new CreatePurchaseTransactionRequest
        {
            Description = "Compra Teste",
            TransactionDate = DateTime.UtcNow,
            AmountUSD = 100m
        };

        var id = await _service.CreateAsync(request);

        // Act + Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ConvertTransactionAsync(id, "ZZZ")); // moeda inválida

        Assert.That(ex!.Message, Does.Contain("Código de moeda inválido"));
    }

    [Test]
    public void Should_Throw_When_Transaction_Not_Found()
    {
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.ConvertTransactionAsync(Guid.NewGuid(), "USD"));

        Assert.That(ex!.Message, Does.Contain("Transação não encontrada"));
    }

    [Test]
    public async Task Should_Throw_When_Rate_Is_Null()
    {
        var customService = new PurchaseTransactionService(
            new InMemoryPurchaseTransactionRepository(),
            new EmptyCurrencyConversionService());

        var request = new CreatePurchaseTransactionRequest
        {
            Description = "Compra sem taxa",
            TransactionDate = DateTime.UtcNow,
            AmountUSD = 10
        };

        var id = await customService.CreateAsync(request);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
            customService.ConvertTransactionAsync(id, "EUR")); // moeda válida

        Assert.That(ex!.Message, Does.Contain("Taxa de câmbio indisponível"));
    }
    
    // Mock que retorna null para qualquer taxa
    private class EmptyCurrencyConversionService : ICurrencyConversionService
    {
        public Task<decimal?> GetExchangeRateAsync(string targetCurrencyCode, DateTime referenceDate)
            => Task.FromResult<decimal?>(null);
    }
}
