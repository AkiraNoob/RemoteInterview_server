using MainService.Application.Exceptions;
using MainService.Application.Slices.MailSlice.Interfaces;
using MainService.Application.Slices.MailSlice.Requests;
using MainService.Application.Slices.NotificationSlice.DTOs;
using MainService.Application.Slices.NotificationSlice.Interfaces;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Enums;
using MainService.Domain.Events;
using MainService.Domain.Models;
using MediatR;

namespace MainService.Application.Slices.RecruitmentSlice.EventHandlers;

//for user
public class ResponseRecruitmentApplicationEventHandler(
    INotificationService notificationService,
    IMailService mailService,
    IUserService userService) : INotificationHandler<EntityUpdatedEvent<UserRecruitment>>
{
    public async Task Handle(EntityUpdatedEvent<UserRecruitment> @event, CancellationToken cancellationToken)
    {
        var userRecruitment = @event.Entity;

        var user = await userService.GetUserDetailAsync(userRecruitment.UserId.ToString(), cancellationToken)
            ?? throw new NotFoundException("User not found.");

        if (userRecruitment.Status != UserRecruitmentStatusEnum.Pending)
        {
            var notification = new NotificationDTO
            {
                UserId = userRecruitment.UserId,
                Type = userRecruitment.Status == UserRecruitmentStatusEnum.Approved ? NotificationTypeEnum.CVGotAccepted : NotificationTypeEnum.CVGotRejected,
                ResourceId = userRecruitment.RecruitmentId,
            };
            await notificationService.NotifyUser(notification, cancellationToken);

            var mailRequest = new MailRequest([user.Email], $"Bài tuyển dụng của bạn đã được {(userRecruitment.Status == UserRecruitmentStatusEnum.Approved ? "chấp thuận" : "từ chối")}.", "Test email");
            await mailService.SendEmailAsync(mailRequest, cancellationToken);
        }
    }
}
