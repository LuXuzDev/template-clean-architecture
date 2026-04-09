using FluentValidation;
using Shared.Results.Errors;
using Shared.Results.Errors.Auth;


namespace Application.Features.Auth.RefreshToken;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithState(_ => new ValidationError(
                Code: AuthError.RefreshTokenRequired.Code,
                Message: AuthError.RefreshTokenRequired.Message,
                PropertyName: "RefreshToken"
                ));
    }
}