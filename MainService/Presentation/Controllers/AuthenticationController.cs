using MainService.Application.Slices.UserSlice.DTOs;
using MainService.Application.Slices.UserSlice.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Presentation.Controllers;
public class AuthenticationController : VersionedApiController
{
    private ITokenService _tokenService;

    public AuthenticationController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }


    [HttpPost("login")]
    [EndpointDescription("Login user in.")]
    public async Task<TokenDTO> LoginAsync([FromBody] TokenRequest body, CancellationToken cancellationToken)
    {
        return await _tokenService.GetTokenAsync(body, cancellationToken);
    }

    [HttpPost("refresh")]
    public async Task<TokenDTO> GetRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        return await _tokenService.RefreshTokenAsync(request, cancellationToken);
    }
}
