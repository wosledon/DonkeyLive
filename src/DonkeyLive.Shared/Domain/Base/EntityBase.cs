namespace DonkeyLive.Shared.Domain.Base;

public abstract class EntityBase : IEntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
