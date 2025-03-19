using MassTransit;

namespace Core.Domain.Contracts;

public abstract class BaseEntity : BaseEntity<Guid>
{
    protected BaseEntity()
    {
        Id = Id == default ? NewId.NextSequentialGuid() : Id;
    }
}

public abstract class BaseEntity<TId>
{
    public TId Id { get; set; } = default!;
}