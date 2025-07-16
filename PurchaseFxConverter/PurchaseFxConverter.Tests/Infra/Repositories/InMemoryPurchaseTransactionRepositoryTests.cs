namespace PurchaseFxConverter.Tests.Infra.Repositories;

public class InMemoryPurchaseTransactionRepositoryTests
{
    [Test]
    public async Task Should_Save_And_Retrieve_Transaction()
    {
        var repository = new InMemoryPurchaseTransactionRepository();
        var transaction = new PurchaseTransaction("Teste", DateTime.UtcNow, 150.00m);

        await repository.SaveAsync(transaction);
        var result = await repository.GetByIdAsync(transaction.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(transaction.Id));
    }

    [Test]
    public async Task Should_Return_Null_If_Transaction_Not_Found()
    {
        var repository = new InMemoryPurchaseTransactionRepository();

        var result = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task Should_Return_Null_If_Not_Found()
    {
        var repository = new InMemoryPurchaseTransactionRepository();
        var result = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.That(result, Is.Null);
    }
}