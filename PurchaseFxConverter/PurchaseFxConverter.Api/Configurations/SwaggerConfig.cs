namespace PurchaseFxConverter.Api.Configurations;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Purchase FX Converter API",
                Version = "v1",
                Description = "API for converting purchases based on US Treasury exchange rates."
            });
        });

        return services;
    }
}