//using MainService.Application.Exceptions;
//using MainService.Application.Interfaces;
//using MainService.Application.Slices.MailSlice.Interfaces;
//using MainService.Application.Slices.MailSlice.Requests;
//using MainService.Application.Slices.NotificationSlice.DTOs;
//using MainService.Application.Slices.NotificationSlice.Interfaces;
//using MainService.Application.Slices.UserSlice.Interfaces;
//using MainService.Domain.Enums;
//using MainService.Domain.Events;
//using MainService.Domain.Models;
//using MediatR;

//namespace MainService.Application.Slices.RecruitmentSlice.EventHandlers;

////for owner
//public class NewApplyRecruitmentEventHandler(
//    INotificationService notificationService,
//    IMailService mailService,
//    IUserService userService,
//    IRepository<Recruitment> recruitmentRepository) : INotificationHandler<EntityCreatedEvent<UserRecruitment>>
//{
//    public async Task Handle(EntityCreatedEvent<UserRecruitment> @event, CancellationToken cancellationToken)
//    {
//        var userRecruitment = @event.Entity;

//        var recruitment = await recruitmentRepository.GetByIdAsync(userRecruitment.RecruitmentId, cancellationToken)
//            ?? throw new NotFoundException("Recruitment not found.");

//        var owner = await userService.GetUserDetailAsync(recruitment.CompanyId.ToString(), cancellationToken)
//            ?? throw new NotFoundException("User not found.");

//        var notification = new NotificationDTO
//        {
//            UserId = recruitment.CompanyId,
//            Type = NotificationTypeEnum.RecruitmentNewApplicant,
//            ResourceId = userRecruitment.RecruitmentId,
//        };
//        await notificationService.NotifyUser(notification, cancellationToken);

//        var mailRequest = new MailRequest([owner.Email], "Một lượt ứng tuyển mới tới bài tuyển dụng của bạn.", "Test email");
//        await mailService.SendEmailAsync(mailRequest, cancellationToken);
//    }
//}
