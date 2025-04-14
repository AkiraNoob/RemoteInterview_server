using MediatR;

namespace MainService.Application.Slices.UserSlice.DTOs;

public class TokenRequest : IRequest<TokenDTO>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
