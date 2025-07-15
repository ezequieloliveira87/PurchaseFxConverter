namespace PurchaseFxConverter.Domain.ValueObjects;

public record CurrencyCode(string Code)
{
    private static readonly HashSet<string> _supportedCurrencies = new()
    {
        "USD", "EUR", "GBP", "BRL", "CAD", "JPY", "CNY"
        // Adicione outras conforme necessário
    };

    public static bool IsValid(string code) =>
        !string.IsNullOrWhiteSpace(code) &&
        _supportedCurrencies.Contains(code.ToUpper());

    public static CurrencyCode From(string code)
    {
        if (!IsValid(code))
            throw new ArgumentException($"Código de moeda inválido: {code}");

        return new CurrencyCode(code.ToUpper());
    }

    public override string ToString() => Code;
}