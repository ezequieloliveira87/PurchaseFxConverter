var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var erros = context.ModelState
            .Where(e => e.Key != "request" && e.Value?.Errors.Count > 0)
            .Select(e => new
            {
                campo = Regex.Replace(e.Key, @"^(\$\.|request\.?)", ""),
                mensagens = e.Value!.Errors.Select(err =>
                    string.IsNullOrWhiteSpace(err.ErrorMessage)
                        ? "Valor inválido ou mal formatado."
                        : err.ErrorMessage)
            });

        return new BadRequestObjectResult(new { erros });
    };
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mapster config
MapsterConfig.RegisterMappings();

// Dependency injection
builder.Services.AddScoped<IPurchaseTransactionService, PurchaseTransactionService>();
builder.Services.AddSingleton<IPurchaseTransactionRepository, InMemoryPurchaseTransactionRepository>();
builder.Services.AddSingleton<ICurrencyConversionService, MockCurrencyConversionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

public partial class Program { }