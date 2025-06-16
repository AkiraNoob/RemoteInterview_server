using Ardalis.Result;
using MainService.Application.Slices.UserSlice.DTOs;
using MainService.Application.Slices.UserSlice.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Presentation.Controllers;

[AllowAnonymous]
public class AuthenticationController(ITokenService tokenService, IUserService userService) : VersionedApiController
{
    [HttpPost("login")]
    [EndpointDescription("Login user in.")]
    public async Task<TokenDTO> LoginAsync([FromBody] TokenRequest body, CancellationToken cancellationToken)
    {
        return await tokenService.GetTokenAsync(body, cancellationToken);
    }

    [HttpPost("refresh")]
    public async Task<TokenDTO> GetRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        return await tokenService.RefreshTokenAsync(request, cancellationToken);
    }

    [HttpPost("sign-up")]
    public async Task<Result<string>> SignUpAsync([FromBody] CreateUserDTO request, CancellationToken cancellationToken)
    {
        return await userService.CreateUserAsync(request, cancellationToken);
    }
}
