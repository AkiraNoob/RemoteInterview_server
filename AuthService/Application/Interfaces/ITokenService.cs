using AuthService.Application.Request;
using AuthService.Application.Response;
using Core.DI.DependencyInjection;
using MessagingAPI.User;

namespace AuthService.Application.Interfaces;

public interface ITokenService : IScopedService
{
    public Task<TokenResponse> GetTokenAsync(VerifyUserLoginCredentialResponseMessage request, CancellationToken cancellationToken);
    public Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
}
