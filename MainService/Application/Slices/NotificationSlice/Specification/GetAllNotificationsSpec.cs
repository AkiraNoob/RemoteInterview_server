using Ardalis.Specification;

namespace MainService.Application.Slices.NotificationSlice.Specification
{
    public class GetAllNotificationsSpec : Specification<Domain.Models.Notification>
    {
        public GetAllNotificationsSpec(Guid userId)
        {
            Query.Where(n => n.UserId == userId)
                 .OrderByDescending(n => n.CreatedOn);
        }
    }
}
