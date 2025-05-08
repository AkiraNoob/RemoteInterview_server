using Ardalis.Specification;
using MainService.Domain.Models;

namespace MainService.Application.Slices.MeetingSlice.Specifications;

public class GetMeetingByIdSpec : Specification<Meeting>
{
    public GetMeetingByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
