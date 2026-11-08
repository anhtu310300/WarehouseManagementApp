using FluentValidation;
using warehouse_api.DTOs.Auth;

namespace warehouse_api.Validators.Auth;

public class LoginRequestValidator
    : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.UserName)
            .NotEmpty()
            .WithMessage("Username is required.");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}