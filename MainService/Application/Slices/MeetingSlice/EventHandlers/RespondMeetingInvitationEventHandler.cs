using MainService.Application.Exceptions;
using MainService.Application.Interfaces;
using MainService.Application.Slices.MailSlice.Interfaces;
using MainService.Application.Slices.MailSlice.Requests;
using MainService.Application.Slices.NotificationSlice.DTOs;
using MainService.Application.Slices.NotificationSlice.Interfaces;
using MainService.Application.Slices.UserSlice.Interfaces;
using MainService.Domain.Enums;
using MainService.Domain.Events;
using MainService.Domain.Models;
using MediatR;

namespace MainService.Application.Slices.MeetingSlice.EventHandlers;

//for owner
public class RespondMeetingInvitationEventHandler(
    INotificationService notificationService,
    IMailService mailService,
    IUserService userService,
    IRepository<Meeting> meetingRepository) : INotificationHandler<EntityUpdatedEvent<UserMeeting>>
{
    public async Task Handle(EntityUpdatedEvent<UserMeeting> @event, CancellationToken cancellationToken)
    {
        var userMeeting = @event.Entity;

        var user = await userService.GetUserDetailAsync(userMeeting.UserId.ToString(), cancellationToken)
            ?? throw new NotFoundException("User not found.");

        var meeting = await meetingRepository.GetByIdAsync(userMeeting.MeetingId, cancellationToken)
            ?? throw new NotFoundException("Meeting not found.");

        var owner = await userService.GetUserDetailAsync(meeting.OwnerId.ToString(), cancellationToken)
            ?? throw new NotFoundException("User not found.");

        var notification = new NotificationDTO
        {
            UserId = meeting.OwnerId,
            Type = userMeeting.Status == UserMeetingStatusEnum.Accepted ? NotificationTypeEnum.PersonAcceptedMeetingInvitation : NotificationTypeEnum.PersonDeclinedMeetingInvitation,
            ResourceId = userMeeting.MeetingId,
        };
        await notificationService.NotifyUser(notification, cancellationToken);

        var mailRequest = new MailRequest([owner.Email],
            $"Lời mời {meeting.Title} tới người dùng {user.FullName} đã {(userMeeting.Status == UserMeetingStatusEnum.Accepted ? "được chấp thuận" : "bị từ chối")}.",
            "Meeting Invitation");
        await mailService.SendEmailAsync(mailRequest, cancellationToken);
    }
}
