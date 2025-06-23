using MainService.Domain.Enums;

namespace MainService.Application.Slices.NotificationSlice.DTOs;

public class NotificationDTO
{
    public Guid UserId { get; set; }
    public NotificationTypeEnum Type { get; set; }
    public Guid ResourceId { get; set; }
}
