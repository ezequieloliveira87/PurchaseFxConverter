namespace PurchaseFxConverter.Application.Mappings;

public abstract class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<PurchaseTransaction, PurchaseTransactionViewModel>
            .NewConfig()
            .Map(member: dest => dest.Id, source: src => src.Id)
            .Map(member: dest => dest.Description, source: src => src.Description)
            .Map(member: dest => dest.TransactionDate, source: src => src.TransactionDate)
            .Map(member: dest => dest.AmountUsd, source: src => src.AmountUsd);
    }
}