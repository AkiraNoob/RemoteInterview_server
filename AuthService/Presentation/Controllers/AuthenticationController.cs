using AuthService.Application.Request;
using AuthService.Application.Response;
using Core.Controller;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Presentation.Controllers;
public class AuthenticationController : VersionedApiController
{
    [HttpPost("/login")]
    [EndpointDescription("Login user in.")]
    public async Task<TokenResponse> LoginAsync([FromBody] TokenRequest body)
    {
        return await Mediator.Send(body);
    }
}
