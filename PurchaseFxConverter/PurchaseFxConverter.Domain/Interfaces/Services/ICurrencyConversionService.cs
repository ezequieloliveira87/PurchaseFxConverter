namespace PurchaseFxConverter.Domain.Interfaces.Services;

public interface ICurrencyConversionService
{
    /// <summary>
    /// Retorna a taxa de câmbio entre USD e uma moeda de destino
    /// com base em uma data de referência
    /// </summary>
    /// <param name="targetCurrencyCode">Código da moeda de destino (ex: EUR)</param>
    /// <param name="referenceDate">Data da transação</param>
    /// <returns>Taxa de câmbio ou null se não encontrada</returns>
    Task<decimal?> GetExchangeRateAsync(string targetCurrencyCode, DateTime referenceDate);
}