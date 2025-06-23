using MainService.Application.Slices.NotificationSlice.DTOs;
using MainService.Application.Slices.NotificationSlice.Interfaces;
using MainService.Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace MainService.Infrastructure.Services;

public class NotificationService(IHubContext<NotificationHub> notificationHub) : INotificationService
{
    public async Task NotifyUser(NotificationDTO payload, CancellationToken cancellationToken = default)
    {
        await notificationHub.Clients
            .User(payload.UserId.ToString())
            .SendAsync(payload.Type.ToString(), payload, cancellationToken);
    }
}
