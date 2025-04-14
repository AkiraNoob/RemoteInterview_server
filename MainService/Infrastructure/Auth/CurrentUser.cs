using MainService.Application.Interfaces;
using System.Security.Claims;
using MainService.Domain.Authorization;

namespace MainService.Infrastructure.Auth;

public class CurrentUser : ICurrentUser, ICurrentUserInitializer
{
    private ClaimsPrincipal? _user;

    private Guid _userId = Guid.Empty;
    public string? Name => _user?.Identity?.Name;

    public IEnumerable<Claim>? GetUserClaims() =>
        _user?.Claims;

    public string? GetUserEmail()
        => IsAuthenticated()
            ? _user!.GetEmail()
            : null;

    public Guid GetUserId()
        => IsAuthenticated()
            ? Guid.Parse(_user?.GetUserId() ?? Guid.Empty.ToString())
            : _userId;

    public string? GetUserPhoneNumber()
        => IsAuthenticated()
            ? _user!.GetPhoneNumber()
            : null;

    public bool IsAuthenticated() =>
        _user?.Identity?.IsAuthenticated is true;

    public bool IsInRole(string role) =>
        _user?.IsInRole(role) is true;

    public void SetCurrentUser(ClaimsPrincipal user)
    {
        if (_user != null)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        _user = user;
    }

    public void SetCurrentUserId(string userId)
    {
        if (_userId != Guid.Empty)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        if (!string.IsNullOrEmpty(userId))
        {
            _userId = Guid.Parse(userId);
        }
    }
}