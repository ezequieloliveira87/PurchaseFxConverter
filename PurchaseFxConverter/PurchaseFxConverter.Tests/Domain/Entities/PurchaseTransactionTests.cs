namespace PurchaseFxConverter.Tests.Domain.Entities;

public class PurchaseTransactionTests
{
    [Test]
    public void Should_Create_Valid_Transaction()
    {
        var transaction = new PurchaseTransaction("Compra válida", DateTime.UtcNow, 150m);
        Assert.That(transaction.IsValid, Is.True);
        Assert.That(Guid.Empty, Is.Not.EqualTo(transaction.Id));
    }

    [Test]
    public void Should_Not_Create_Invalid_Transaction()
    {
        var transaction = new PurchaseTransaction("", DateTime.UtcNow.AddDays(1), 0m);
        Assert.That(transaction.IsValid, Is.False);
    }
    
    [Test]
    public void Should_Return_TransactionDate_As_Provided()
    {
        var expectedDate = new DateTime(2024, 12, 25);
        var transaction = new PurchaseTransaction("Compra de Natal", expectedDate, 250.00m);

        Assert.That(transaction.TransactionDate, Is.EqualTo(expectedDate));
    }

    [Test]
    public void Should_Trim_Description()
    {
        var transaction = new PurchaseTransaction("  Teste  ", DateTime.UtcNow, 10m);
        Assert.That("Teste", Is.EqualTo(transaction.Description));
    }

    [Test]
    public void Should_Round_Amount()
    {
        var transaction = new PurchaseTransaction("Compra", DateTime.UtcNow, 10.123m);
        Assert.That(10.12m, Is.EqualTo(transaction.AmountUSD));
    }
}