using MediatR;

namespace MainService.Domain.Events;

public class EntitySoftDeletedEvent<TEntity> : INotification
    where TEntity : class
{
    public EntitySoftDeletedEvent(TEntity entity) => Entity = entity;

    public TEntity Entity { get; }

    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}