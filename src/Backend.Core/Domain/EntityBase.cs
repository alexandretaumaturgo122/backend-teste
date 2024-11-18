namespace Backend.Core.Domain;

public abstract class EntityBase<TId> : ITimestampEntity
{
    public TId Id { get; protected internal set; } = default!;
    public DateTimeOffset CreatedAt { get; protected internal set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; protected internal set; }
}

public abstract class EntityBase : EntityBase<Guid>
{
    protected EntityBase()
    {
        Id = Guid.NewGuid();
    }
}