using MainService.Domain.Contracts;

namespace AuthService.Domain.Models;

public class Token : AuditableEntity, IAggregateRoot
{
    public string UserId { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expires { get; set; }
}