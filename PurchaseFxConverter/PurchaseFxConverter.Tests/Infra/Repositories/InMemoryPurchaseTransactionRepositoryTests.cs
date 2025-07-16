namespace PurchaseFxConverter.Tests.Infra.Repositories;

public class InMemoryPurchaseTransactionRepositoryTests
{
    [Test]
    public async Task Should_Save_And_Retrieve_Transaction()
    {
        var repo = new InMemoryPurchaseTransactionRepository();
        var transaction = new PurchaseTransaction("Teste", DateTime.UtcNow, 150.00m);

        await repo.SaveAsync(transaction);
        var result = await repo.GetByIdAsync(transaction.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(transaction.Id));
    }
    
    [Test]
    public async Task Should_Return_Null_If_Transaction_Not_Found()
    {
        // Arrange
        var repo = new InMemoryPurchaseTransactionRepository();

        // Act
        var result = await repo.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task Should_Return_Null_If_Not_Found()
    {
        var repo = new InMemoryPurchaseTransactionRepository();
        var result = await repo.GetByIdAsync(Guid.NewGuid());

        Assert.That(result, Is.Null);
    }
}