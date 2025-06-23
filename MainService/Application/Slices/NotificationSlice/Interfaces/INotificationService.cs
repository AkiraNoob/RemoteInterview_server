using MainService.Application.Interfaces;
using MainService.Application.Slices.NotificationSlice.DTOs;

namespace MainService.Application.Slices.NotificationSlice.Interfaces;

public interface INotificationService : IScopedService
{
    public Task NotifyUser(NotificationDTO payload, CancellationToken cancellationToken = default);
}
