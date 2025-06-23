namespace MainService.Application.Slices.MeetingSlice.DTOs;

public class ShortenMeetingDTO
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Guid RecruitmentId { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
}
