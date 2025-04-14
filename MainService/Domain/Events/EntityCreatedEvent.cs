using MediatR;

namespace MainService.Domain.Events;

public class EntityCreatedEvent<TEntity> : INotification
    where TEntity : class
{
    public EntityCreatedEvent(TEntity entity) => Entity = entity;

    public TEntity Entity { get; }

    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}