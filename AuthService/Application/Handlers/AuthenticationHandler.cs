using AuthService.Application.Request;
using AuthService.Application.Response;
using MassTransit;
using MediatR;
using MessagingAPI.User;

namespace AuthService.Application.Handlers;


public class AuthenticationHandler : IRequestHandler<TokenRequest, TokenResponse>
{
    private readonly IRequestClient<VerifyUserLoginCredentialRequestMessage> _verifyUserLoginCredentialClient;

    public AuthenticationHandler(IRequestClient<VerifyUserLoginCredentialRequestMessage> verifyUserLoginCredentialClient)
    {
        _verifyUserLoginCredentialClient = verifyUserLoginCredentialClient;
    }

    public async Task<TokenResponse> Handle(TokenRequest request, CancellationToken cancellationToken)
    {
        var response = await _verifyUserLoginCredentialClient.GetResponse<VerifyUserLoginCredentialResponseMessage>(new VerifyUserLoginCredentialRequestMessage()
        {
            Email = request.Email,
            Password = request.Password,
        }, cancellationToken);

    }
}
