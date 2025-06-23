using MainService.Application.Slices.UserSlice.DTOs;

namespace MainService.Application.Slices.MeetingSlice.DTOs;

public class DetailedMeetingDTO
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Guid RecruitmentId { get; set; }
    public string RecruitmentTitle { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
    public ICollection<ShortenUserDetailDTO> Participants { get; set; }
}
