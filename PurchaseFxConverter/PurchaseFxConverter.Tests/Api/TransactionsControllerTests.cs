namespace PurchaseFxConverter.Tests.Api;

[TestFixture]
public class TransactionsControllerTests
{
    [SetUp]
    public void SetUp()
    {
        _serviceMock = new Mock<IPurchaseTransactionService>();
        _controller = new TransactionsController(_serviceMock.Object);
    }
    private Mock<IPurchaseTransactionService> _serviceMock;
    private TransactionsController _controller;

    [Test]
    public async Task Create_ShouldReturnCreated_WhenValidRequest()
    {
        var request = new CreatePurchaseTransactionRequest
        {
            Description = "Teste",
            TransactionDate = DateTime.UtcNow,
            AmountUsd = 100
        };

        var generatedId = Guid.NewGuid();

        _serviceMock
            .Setup(s => s.CreateAsync(request))
            .ReturnsAsync(generatedId);

        var result = await _controller.Create(request) as CreatedAtActionResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.ActionName, Is.EqualTo(nameof(TransactionsController.GetById)));

        var value = result.Value;
        var property = value!.GetType().GetProperty("id");
        var id = property?.GetValue(value);

        Assert.That(id, Is.EqualTo(generatedId));
    }

    [Test]
    public async Task Create_ShouldReturnBadRequest_WhenArgumentException()
    {
        var request = new CreatePurchaseTransactionRequest();
        var expectedMessage = "Invalid";

        _serviceMock
            .Setup(s => s.CreateAsync(request))
            .ThrowsAsync(new ArgumentException(expectedMessage));

        var result = await _controller.Create(request) as BadRequestObjectResult;

        Assert.That(result, Is.Not.Null);

        var value = result!.Value;
        var property = value!.GetType().GetProperty("error");
        var errorMessage = property?.GetValue(value) as string;

        Assert.That(errorMessage, Is.EqualTo(expectedMessage));
    }

    [Test]
    public async Task GetById_ShouldReturnOk_WhenTransactionExists()
    {
        var id = Guid.NewGuid();
        var transaction = new PurchaseTransactionViewModel
        {
            Id = id,
            Description = "Transaction Test",
            TransactionDate = DateTime.UtcNow,
            AmountUsd = 100
        };

        _serviceMock
            .Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync(transaction);

        var result = await _controller.GetById(id);

        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);

        var returnedModel = okResult!.Value as PurchaseTransactionViewModel;
        Assert.That(returnedModel, Is.Not.Null);
        Assert.That(returnedModel!.Id, Is.EqualTo(id));
    }

    [Test]
    public async Task GetById_ShouldReturnNotFound_WhenTransactionIsNull()
    {
        var id = Guid.NewGuid();

        _serviceMock
            .Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync((PurchaseTransactionViewModel?)null);

        var result = await _controller.GetById(id);
        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task Convert_ShouldReturnOk_WhenConversionSuccessful()
    {
        var id = Guid.NewGuid();
        var converted = new ConvertedTransactionViewModel
        {
            Id = id,
            Currency = "Brazil-Real",
            ConvertedAmount = 500,
            ExchangeRate = 5
        };

        _serviceMock
            .Setup(s => s.ConvertTransactionAsync(id, "Brazil-Real"))
            .ReturnsAsync(converted);

        var result = await _controller.Convert(id, "Brazil-Real") as OkObjectResult;
        var value = result!.Value as ConvertedTransactionViewModel;

        Assert.That(result, Is.Not.Null);
        Assert.That(value, Is.Not.Null);
        Assert.That(value!.Currency, Is.EqualTo("Brazil-Real"));
    }

    [Test]
    public async Task Convert_ShouldReturnBadRequest_WhenArgumentException()
    {
        var id = Guid.NewGuid();
        var expectedMessage = "Invalid currency code";

        _serviceMock
            .Setup(s => s.ConvertTransactionAsync(id, "Brazil-Real"))
            .ThrowsAsync(new ArgumentException(expectedMessage));

        var result = await _controller.Convert(id, "Brazil-Real") as BadRequestObjectResult;

        Assert.That(result, Is.Not.Null);

        var value = result!.Value;
        var property = value!.GetType().GetProperty("error");
        var errorMessage = property?.GetValue(value) as string;

        Assert.That(errorMessage, Is.EqualTo(expectedMessage));
    }

    [Test]
    public async Task Convert_ShouldReturnNotFound_WhenInvalidOperationException()
    {
        var id = Guid.NewGuid();
        var expectedMessage = "Transaction not found";

        _serviceMock
            .Setup(s => s.ConvertTransactionAsync(id, "Brazil-Real"))
            .ThrowsAsync(new InvalidOperationException(expectedMessage));

        var result = await _controller.Convert(id, "Brazil-Real") as NotFoundObjectResult;

        Assert.That(result, Is.Not.Null);

        var value = result!.Value;
        var property = value!.GetType().GetProperty("error");
        var errorMessage = property?.GetValue(value) as string;

        Assert.That(errorMessage, Is.EqualTo(expectedMessage));
    }
}