﻿namespace MainService.Application.Slices.MeetingSlice.DTOs;

public class UpdateMeetingDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
