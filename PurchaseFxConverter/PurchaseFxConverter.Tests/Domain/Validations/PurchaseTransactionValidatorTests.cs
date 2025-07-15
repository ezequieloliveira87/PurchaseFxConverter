namespace PurchaseFxConverter.Tests.Domain.Validations;

public class PurchaseTransactionValidatorTests
{
    [Test]
    public void Should_Validate_Correct_Data()
    {
        var validator = new PurchaseTransactionValidator("Compra válida", DateTime.UtcNow, 100.00m);
        Assert.That(validator.IsValid);
    }

    [Test]
    public void Should_Fail_When_Description_Too_Long()
    {
        var longDescription = new string('A', 51);
        var validator = new PurchaseTransactionValidator(longDescription, DateTime.UtcNow, 100.00m);
        Assert.That(validator.IsValid, Is.False);
    }

    [Test]
    public void Should_Fail_When_Amount_Is_Zero()
    {
        var validator = new PurchaseTransactionValidator("Compra", DateTime.UtcNow, 0m);
        Assert.That(validator.IsValid, Is.False);
    }

    [Test]
    public void Should_Fail_When_Date_Is_Future()
    {
        var futureDate = DateTime.UtcNow.AddDays(1);
        var validator = new PurchaseTransactionValidator("Compra", futureDate, 100m);
        Assert.That(validator.IsValid, Is.False);
    }
}