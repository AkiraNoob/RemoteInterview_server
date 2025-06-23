namespace MainService.Domain.Enums;

public enum NotificationTypeEnum
{
    //for user
    ReceiveMeetingInvitation,
    CVGotAccepted,
    CVGotRejected,

    //for admin
    PersonAcceptedMeetingInvitation,
    PersonDeclinedMeetingInvitation,
    RecruitmentNewApplicant,
    NewReview
}
