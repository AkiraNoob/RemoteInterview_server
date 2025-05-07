using Ardalis.Specification;
using MainService.Domain.Models;

namespace MainService.Application.Slices.MeetingSlice.Specifications;

public class IncludedGetMeetingByIdSpec : Specification<Meeting>
{
    public IncludedGetMeetingByIdSpec(Guid meetingId)
    {
        Query.Include(x => x.UserMeetings)
             .Include(x => x.Recruitment)
             .Where(x => x.Id == meetingId);
    }
}
