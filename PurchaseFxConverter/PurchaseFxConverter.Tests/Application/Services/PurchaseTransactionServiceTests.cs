namespace PurchaseFxConverter.Tests.Application.Services;

[TestFixture]
public class PurchaseTransactionServiceTests
{

    [SetUp]
    public void Setup()
    {
        _repoMock = new Mock<IPurchaseTransactionRepository>();
        _currencyServiceMock = new Mock<ITreasuryCurrencyConversionService>();
        _loggerMock = new Mock<ILogger<PurchaseTransactionService>>();

        _service = new PurchaseTransactionService(_repoMock.Object, _currencyServiceMock.Object, _loggerMock.Object);
    }
    private Mock<IPurchaseTransactionRepository> _repoMock;
    private Mock<ITreasuryCurrencyConversionService> _currencyServiceMock;
    private Mock<ILogger<PurchaseTransactionService>> _loggerMock;
    private PurchaseTransactionService _service;

    [Test]
    public async Task CreateAsync_WithValidTransaction_ShouldReturnId_AndLogInfo()
    {
        // Arrange
        var request = new CreatePurchaseTransactionRequest
        {
            Description = "Compra Teste",
            AmountUsd = 100,
            TransactionDate = DateTime.UtcNow
        };

        // Act
        var id = await _service.CreateAsync(request);

        // Assert
        Assert.That(id, Is.Not.EqualTo(Guid.Empty));
        _repoMock.Verify(expression: r => r.SaveAsync(It.IsAny<PurchaseTransaction>()), Times.Once);

        _loggerMock.Verify(
            expression: x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(SuccessMessage.TransactionCreated.GetEnumDescription())),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    public void CreateAsync_WithInvalidTransaction_ShouldThrowArgumentException()
    {
        // Arrange
        var request = new CreatePurchaseTransactionRequest
        {
            Description = "",// inválido
            AmountUsd = 0,
            TransactionDate = DateTime.UtcNow.AddDays(1)// futuro
        };

        // Act + Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(request));
        Assert.That(ex!.Message, Does.Contain("A descrição é obrigatória"));
    }

    [Test]
    public async Task GetByIdAsync_WhenTransactionExists_ShouldReturnViewModel()
    {
        // Arrange
        var transaction = new PurchaseTransaction("Compra", DateTime.UtcNow, 123);
        _repoMock.Setup(r => r.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);

        // Act
        var result = await _service.GetByIdAsync(transaction.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Description, Is.EqualTo("Compra"));
    }

    [Test]
    public async Task GetByIdAsync_WhenTransactionDoesNotExist_ShouldReturnNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((PurchaseTransaction)null!);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        Assert.That(result, Is.Null);
    }

    [Test]
    public void ConvertTransactionAsync_WhenTransactionNotFound_ShouldThrow()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((PurchaseTransaction)null!);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.ConvertTransactionAsync(Guid.NewGuid(), "EUR"));

        Assert.That(ex!.Message, Does.Contain(ErrorMessage.TransactionNotFound.GetEnumDescription()));
    }

    [Test]
    public async Task ConvertTransactionAsync_WhenRateIsNull_ShouldThrow()
    {
        var transaction = new PurchaseTransaction("Compra", DateTime.UtcNow, 50);
        _repoMock.Setup(r => r.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);
        _currencyServiceMock.Setup(s => s.GetExchangeRateAsync("EUR", transaction.TransactionDate))
            .ReturnsAsync((decimal?)null);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.ConvertTransactionAsync(transaction.Id, "EUR"));

        Assert.That(ex!.Message, Does.Contain(ErrorMessage.ExchangeRateUnavailable.GetEnumDescription()));
    }

    [Test]
    public async Task ConvertTransactionAsync_WithValidRate_ShouldReturnConvertedViewModel()
    {
        var transaction = new PurchaseTransaction("Compra OK", DateTime.UtcNow, 100);
        _repoMock.Setup(r => r.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);
        _currencyServiceMock.Setup(s => s.GetExchangeRateAsync("EUR", transaction.TransactionDate))
            .ReturnsAsync(5.4321m);

        var result = await _service.ConvertTransactionAsync(transaction.Id, "EUR");

        Assert.That(result.Currency, Is.EqualTo("EUR"));
        Assert.That(result.ExchangeRate, Is.EqualTo(5.4321m));
        Assert.That(result.ConvertedAmount, Is.EqualTo(543.21m));
    }
}