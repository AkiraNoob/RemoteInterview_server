using MediatR;

namespace MainService.Domain.Events;

public class EntityDeletedEvent<TEntity> : INotification
    where TEntity : class
{
    public EntityDeletedEvent(TEntity entity) => Entity = entity;

    public TEntity Entity { get; }

    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}