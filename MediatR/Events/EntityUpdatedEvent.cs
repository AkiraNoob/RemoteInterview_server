using MediatR;

namespace Core.MediatR.Events;

public class EntityUpdatedEvent<TEntity> : INotification
    where TEntity : class
{
    public EntityUpdatedEvent(TEntity entity) => Entity = entity;

    public TEntity Entity { get; }

    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}