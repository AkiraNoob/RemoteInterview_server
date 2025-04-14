using System.Security.Claims;

namespace MainService.Application.Interfaces;
public interface ICurrentUser
{
    string? Name { get; }
    IEnumerable<Claim>? GetUserClaims();
    string? GetUserEmail();
    Guid GetUserId();
    string? GetUserPhoneNumber();
    bool IsAuthenticated();
    bool IsInRole(string role);
}