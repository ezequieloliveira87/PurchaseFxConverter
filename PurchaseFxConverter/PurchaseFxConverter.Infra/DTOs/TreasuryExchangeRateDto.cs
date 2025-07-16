namespace PurchaseFxConverter.Infra.DTOs;

public class TreasuryExchangeRateDto
{
    public string CountryCurrencyDesc { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; }
    public DateTime RecordDate { get; set; }
}