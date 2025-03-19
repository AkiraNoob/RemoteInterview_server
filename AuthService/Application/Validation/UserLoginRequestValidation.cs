using AuthService.Application.Request;
using FluentValidation;

namespace AuthService.Application.Validation;

public class UserLoginRequestValidation : CustomValidator<TokenRequest>
{
    public UserLoginRequestValidation()
    {
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
