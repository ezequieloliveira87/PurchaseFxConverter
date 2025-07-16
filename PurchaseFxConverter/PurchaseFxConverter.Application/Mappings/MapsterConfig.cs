namespace PurchaseFxConverter.Application.Mappings;

public class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<PurchaseTransaction, PurchaseTransactionViewModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.TransactionDate, src => src.TransactionDate)
            .Map(dest => dest.AmountUSD, src => src.AmountUSD);
    }
}