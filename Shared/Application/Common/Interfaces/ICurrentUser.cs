using System.Security.Claims;

namespace Shared.Application.Common.Interfaces;

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