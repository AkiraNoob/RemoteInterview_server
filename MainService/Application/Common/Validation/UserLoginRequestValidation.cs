using FluentValidation;
using MainService.Application.Slices.UserSlice.DTOs;

namespace AuthService.Application.Common.Validation;

public class UserLoginRequestValidation : CustomValidator<TokenRequest>
{
    public UserLoginRequestValidation()
    {
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
