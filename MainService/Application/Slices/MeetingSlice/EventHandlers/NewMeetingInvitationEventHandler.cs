//using MainService.Application.Exceptions;
//using MainService.Application.Slices.MailSlice.Interfaces;
//using MainService.Application.Slices.MailSlice.Requests;
//using MainService.Application.Slices.NotificationSlice.DTOs;
//using MainService.Application.Slices.NotificationSlice.Interfaces;
//using MainService.Application.Slices.UserSlice.Interfaces;
//using MainService.Domain.Enums;
//using MainService.Domain.Events;
//using MainService.Domain.Models;
//using MediatR;

//namespace MainService.Application.Slices.MeetingSlice.EventHandlers;

////for user
//public class NewMeetingInvitationEventHandler(
//    INotificationService notificationService,
//    IMailService mailService,
//    IUserService userService) : INotificationHandler<EntityCreatedEvent<UserMeeting>>
//{
//    public async Task Handle(EntityCreatedEvent<UserMeeting> @event, CancellationToken cancellationToken)
//    {
//        var userMeeting = @event.Entity;

//        if (userMeeting.Role == Domain.Enums.MeetingRoleEnum.Owner)
//            return;

//        var user = await userService.GetUserDetailAsync(userMeeting.UserId.ToString(), cancellationToken)
//            ?? throw new NotFoundException("User not found.");


//        var notification = new NotificationDTO
//        {
//            UserId = userMeeting.UserId,
//            Type = NotificationTypeEnum.ReceiveMeetingInvitation,
//            ResourceId = userMeeting.MeetingId,
//        };
//        await notificationService.NotifyUser(notification, cancellationToken);

//        var mailRequest = new MailRequest([user.Email], "Bạn có lời mời tham gia cuộc họp.", "Meeting Invitation");
//        await mailService.SendEmailAsync(mailRequest, cancellationToken);
//    }
//}
