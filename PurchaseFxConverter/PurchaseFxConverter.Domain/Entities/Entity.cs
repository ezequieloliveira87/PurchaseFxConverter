namespace PurchaseFxConverter.Domain.Entities;

public abstract class Entity : Notifiable<Notification>
{
    public Guid Id { get; protected set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}