namespace PurchaseFxConverter.Domain.Enums.Messages;

public enum ErrorMessage
{
    [Description("Transaction not found.")]
    TransactionNotFound,

    [Description("Exchange rate unavailable for the given date.")]
    ExchangeRateUnavailable,

    [Description("Request failed: ")]
    TransactionRequestError
}