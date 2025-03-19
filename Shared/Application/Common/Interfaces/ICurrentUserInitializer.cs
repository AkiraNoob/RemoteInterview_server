using System.Security.Claims;

namespace Shared.Application.Common.Interfaces;

public interface ICurrentUserInitializer
{
    void SetCurrentUser(ClaimsPrincipal user);

    void SetCurrentUserId(string userId);
}