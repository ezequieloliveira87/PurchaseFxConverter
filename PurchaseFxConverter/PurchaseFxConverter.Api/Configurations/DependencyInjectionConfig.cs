namespace PurchaseFxConverter.Api.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddProjectDependencies(this IServiceCollection services)
    {
        services.AddScoped<IPurchaseTransactionService, PurchaseTransactionService>();
        services.AddSingleton<ITreasuryCurrencyConversionService, TreasuryCurrencyConversionService>();

        services.AddSingleton<IPurchaseTransactionRepository, InMemoryPurchaseTransactionRepository>();

        services.AddHttpClient<ITreasuryCurrencyConversionService, TreasuryCurrencyConversionService>();

        return services;
    }
}