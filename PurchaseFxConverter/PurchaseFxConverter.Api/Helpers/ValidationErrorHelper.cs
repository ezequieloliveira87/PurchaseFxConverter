namespace PurchaseFxConverter.Api.Helpers;

public static class ValidationErrorHelper
{
    public static IActionResult FromFlunt(IReadOnlyCollection<Notification> notifications)
    {
        var errors = notifications
            .GroupBy(n => n.Key)
            .Select(g => new
            {
                campo = g.Key,
                mensagens = g.Select(n => n.Message).ToArray()
            });

        return new BadRequestObjectResult(new { erros = errors });
    }
}
