using MainService.Application.Slices.NotificationSlice.DTOs;
using MainService.Application.Slices.NotificationSlice.Interfaces;
using MainService.Domain.Models;
using MainService.Infrastructure.Persistence.Context;
using MainService.Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace MainService.Infrastructure.Services;

public class NotificationService(IHubContext<NotificationHub> notificationHub, ApplicationDbContext applicationDbContext) : INotificationService
{
    public async Task NotifyUser(NotificationDTO payload, CancellationToken cancellationToken = default)
    {
        var notification = new Notification()
        {
            UserId = payload.UserId,
            Type = payload.Type,
            ResourceId = payload.ResourceId,
        };

        await applicationDbContext.Notification.AddAsync(notification, cancellationToken);

        await notificationHub.Clients
            .User(payload.UserId.ToString())
            .SendAsync(payload.Type.ToString(), payload, cancellationToken);
    }
}
