var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.InvalidModelStateResponseFactory = context => {
        var errors = context.ModelState
            .Where(e => e.Key != "request" && e.Value?.Errors.Count > 0)
            .Select(e => new
            {
                campo = Regex.Replace(e.Key, @"^(\$\.|request\.?)", ""),
                mensagens = e.Value!.Errors.Select(err =>
                    string.IsNullOrWhiteSpace(err.ErrorMessage)
                        ? "The value is invalid or has an incorrect format."
                        : err.ErrorMessage)
            });

        return new BadRequestObjectResult(new
        {
            errors
        });
    };
});

builder.Services.AddSwaggerConfiguration();
builder.Services.AddProjectDependencies();

MapsterConfig.RegisterMappings();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

public abstract partial class Program;