namespace PurchaseFxConverter.Tests.Domain.ValueObjects;

public class CurrencyCodeTests
{
    [TestCase("usd")]
    [TestCase("EUR")]
    [TestCase("brl")]
    public void Should_Create_Valid_CurrencyCode_From_String(string code)
    {
        var currency = CurrencyCode.From(code);

        Assert.That(currency.Code, Is.EqualTo(code.ToUpper()));
        Assert.That(currency.ToString(), Is.EqualTo(code.ToUpper()));
    }

    [Test]
    public void Should_Throw_Exception_For_Invalid_Code()
    {
        var ex = Assert.Throws<ArgumentException>(() => CurrencyCode.From("ZZZ"));
        Assert.That(ex?.Message, Does.Contain("Código de moeda inválido"));
    }

    [Test]
    public void Should_Validate_Supported_Currencies()
    {
        Assert.That(CurrencyCode.IsValid("EUR"), Is.True);
        Assert.That(CurrencyCode.IsValid("XXX"), Is.False);
        Assert.That(CurrencyCode.IsValid(""), Is.False);
        Assert.That(CurrencyCode.IsValid("   "), Is.False);
    }
}