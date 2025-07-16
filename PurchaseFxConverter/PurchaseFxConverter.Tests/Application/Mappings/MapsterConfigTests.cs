namespace PurchaseFxConverter.Tests.Application.Mappings;

public class MapsterConfigTests
{
    [SetUp]
    public void Setup()
    {
        MapsterConfig.RegisterMappings();
    }

    [Test]
    public void Should_Map_PurchaseTransaction_To_ViewModel()
    {
        var transaction = new PurchaseTransaction("Mapping Validation Test", DateTime.UtcNow, 99.99m);

        var viewModel = transaction.Adapt<PurchaseTransactionViewModel>();

        Assert.That(viewModel, Is.Not.Null);
        Assert.That(viewModel.Id, Is.EqualTo(transaction.Id));
        Assert.That(viewModel.Description, Is.EqualTo(transaction.Description));
        Assert.That(viewModel.AmountUsd, Is.EqualTo(transaction.AmountUsd));
    }
}