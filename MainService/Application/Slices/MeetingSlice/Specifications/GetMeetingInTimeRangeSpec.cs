using Ardalis.Specification;
using MainService.Domain.Models;

namespace MainService.Application.Slices.MeetingSlice.Specifications;

public class GetMeetingInTimeRangeSpec : Specification<Meeting>
{
    public GetMeetingInTimeRangeSpec(DateTime startTime, DateTime endTime)
    {
        Query.Where(x => x.StartTime >= startTime && x.EndTime <= endTime);
    }
}

public class GetMeetingByStartTimeRangeSpec : Specification<Meeting>
{
    public GetMeetingByStartTimeRangeSpec(DateTime startTime)
    {
        Query.Where(x => x.StartTime >= startTime);
    }
}

public class GetMeetingByEndTimeRangeSpec : Specification<Meeting>
{
    public GetMeetingByEndTimeRangeSpec(DateTime endTime)
    {
        Query.Where(x => x.EndTime <= endTime);
    }
}
