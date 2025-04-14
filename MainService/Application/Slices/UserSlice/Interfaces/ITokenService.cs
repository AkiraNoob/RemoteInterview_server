using MainService.Application.Interfaces;
using MainService.Application.Slices.UserSlice.DTOs;

namespace MainService.Application.Slices.UserSlice.Interfaces;

public interface ITokenService : IScopedService
{
    public Task<TokenDTO> GetTokenAsync(TokenRequest request, CancellationToken cancellationToken);
    public Task<TokenDTO> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
}
