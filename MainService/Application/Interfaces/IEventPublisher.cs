using MediatR;

namespace MainService.Application.Interfaces;

public interface IEventPublisher : IScopedService
{
    Task PublishAsync(INotification notification, CancellationToken cancellationToken);
}
