using AuthService.Application.Response;
using MediatR;

namespace AuthService.Application.Request;

public class TokenRequest : IRequest<TokenResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
