using MediatR;

namespace MainService.Domain.Events;

public class EntityRestoredEvent<TEntity> : INotification
    where TEntity : class
{
    public EntityRestoredEvent(TEntity entity) => Entity = entity;

    public TEntity Entity { get; }

    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}