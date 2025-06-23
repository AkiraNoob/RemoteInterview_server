using MainService.Application.Interfaces;
using MainService.Application.Slices.NotificationSlice.DTOs;
using MainService.Application.Slices.NotificationSlice.Specification;
using MainService.Domain.Models;
using Mapster;
using MediatR;

namespace MainService.Application.Slices.NotificationSlice.Requests;

public class GetAllNotificationRequest : IRequest<ICollection<NotificationDTO>>
{

}

public class GetAllNotificationRequestHandler(IRepository<Notification> notificationRepository, ICurrentUser currentUser) : IRequestHandler<GetAllNotificationRequest, ICollection<NotificationDTO>>
{
    public async Task<ICollection<NotificationDTO>> Handle(GetAllNotificationRequest request, CancellationToken cancellationToken)
    {
        var userId = currentUser.GetUserId();

        var notifications = (await notificationRepository.ListAsync(
            new GetAllNotificationsSpec(userId),
            cancellationToken
        )).Adapt<List<NotificationDTO>>();

        return notifications;
    }
}
