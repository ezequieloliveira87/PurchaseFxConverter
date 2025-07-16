namespace PurchaseFxConverter.Infra.Services;

public class TreasuryCurrencyConversionService : ITreasuryCurrencyConversionService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TreasuryCurrencyConversionService> _logger;

    public TreasuryCurrencyConversionService(HttpClient httpClient, IConfiguration configuration, ILogger<TreasuryCurrencyConversionService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        var baseUrl = configuration.GetValue<string>("ExternalApis:TreasuryRates:BaseUrl")
                      ?? throw new InvalidOperationException("TreasuryRatesBaseUrl is missing");

        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<decimal?> GetExchangeRateAsync(string targetCurrencyCode, DateTime transactionDate)
    {
        var startDate = transactionDate.AddMonths(-6).ToString("yyyy-MM-dd");
        var endDate = transactionDate.ToString("yyyy-MM-dd");
        var encodedCurrency = Uri.EscapeDataString(targetCurrencyCode);

        var endpoint = $"/services/api/fiscal_service/v1/accounting/od/rates_of_exchange?" +
                       $"fields=country_currency_desc,exchange_rate,record_date&" +
                       $"filter=country_currency_desc:in:({encodedCurrency}),record_date:gte:{startDate},record_date:lte:{endDate}&" +
                       $"sort=-record_date&page[size]=1";

        var response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("[ERROR]:  " + ErrorMessage.TransactionRequestError.GetEnumDescription() + $"{response.StatusCode}");
            return null;
        }

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var json = await JsonDocument.ParseAsync(stream);

        var exchangeRate = json.RootElement
            .GetProperty("data")
            .EnumerateArray()
            .FirstOrDefault();

        if (exchangeRate.ValueKind != JsonValueKind.Object)
        {
            _logger.LogWarning("[WARNING] " + ErrorMessage.ExchangeRateUnavailable.GetEnumDescription());
            return null;
        }

        var dto = new TreasuryExchangeRateDto
        {
            CountryCurrencyDesc = exchangeRate.GetProperty("country_currency_desc").GetString() ?? "",
            ExchangeRate = decimal.Parse(exchangeRate.GetProperty("exchange_rate").GetString() ?? "0", CultureInfo.InvariantCulture),
            RecordDate = DateTime.Parse(exchangeRate.GetProperty("record_date").GetString() ?? "")
        };

        return Math.Round(dto.ExchangeRate, 4);
    }
}