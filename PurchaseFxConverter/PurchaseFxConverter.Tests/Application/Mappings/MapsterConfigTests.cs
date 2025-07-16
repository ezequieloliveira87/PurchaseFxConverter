namespace PurchaseFxConverter.Tests.Application.Mappings;

public class MapsterConfigTests
{
    [SetUp]
    public void Setup()
    {
        MapsterConfig.RegisterMappings(); // Força o carregamento
    }

    [Test]
    public void Should_Map_PurchaseTransaction_To_ViewModel()
    {
        // Arrange
        var transaction = new PurchaseTransaction("Teste Mapping", DateTime.UtcNow, 99.99m);

        // Act
        var viewModel = transaction.Adapt<PurchaseTransactionViewModel>();

        // Assert
        Assert.That(viewModel, Is.Not.Null);
        Assert.That(viewModel.Id, Is.EqualTo(transaction.Id));
        Assert.That(viewModel.Description, Is.EqualTo(transaction.Description));
        Assert.That(viewModel.AmountUSD, Is.EqualTo(transaction.AmountUSD));
    }
}