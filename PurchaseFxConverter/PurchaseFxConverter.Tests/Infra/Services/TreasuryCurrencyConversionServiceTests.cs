namespace PurchaseFxConverter.Tests.Infra.Services;

public class TreasuryCurrencyConversionServiceTests
{
    private IConfiguration _configuration;
    private HttpClient _httpClient;
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private Mock<ILogger<TreasuryCurrencyConversionService>> _loggerMock;

    [SetUp]
    public void Setup()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

        var mockSettings = new Dictionary<string, string>
        {
            {
                "ExternalApis:TreasuryRates:BaseUrl", "https://api.fiscaldata.treasury.gov/"
            }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(mockSettings!)
            .Build();

        _loggerMock = new Mock<ILogger<TreasuryCurrencyConversionService>>();
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
        if (_configuration is IDisposable disposableConfig)
            disposableConfig.Dispose();
    }

    [Test]
    public async Task Should_Return_ExchangeRate_When_Successful()
    {
        var jsonResponse = @"
        {
            ""data"": [{
                ""country_currency_desc"": ""Brazil-Real"",
                ""exchange_rate"": ""5.678"",
                ""record_date"": ""2024-06-30""
            }]
        }";

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            });

        var service = new TreasuryCurrencyConversionService(_httpClient, _configuration, _loggerMock.Object);
        var exchangeRate = await service.GetExchangeRateAsync("Brazil-Real", DateTime.Parse("2024-06-30"));

        Assert.That(exchangeRate, Is.EqualTo(5.678m));
    }

    [Test]
    public async Task Should_Return_Null_When_Response_Is_Not_Successful()
    {
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        var service = new TreasuryCurrencyConversionService(_httpClient, _configuration, _loggerMock.Object);
        var exchangeRate = await service.GetExchangeRateAsync("Brazil-Real", DateTime.Now);

        Assert.That(exchangeRate, Is.Null);
    }

    [Test]
    public async Task Should_Return_Null_When_Data_Is_Empty()
    {
        var jsonResponse = @"{ ""data"": [] }";

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            });

        var service = new TreasuryCurrencyConversionService(_httpClient, _configuration, _loggerMock.Object);
        var exchangeRate = await service.GetExchangeRateAsync("Brazil-Real", DateTime.Now);

        Assert.That(exchangeRate, Is.Null);
    }
}