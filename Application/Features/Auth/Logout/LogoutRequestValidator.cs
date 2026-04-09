using Application.Features.Auth.RefreshToken;
using FluentValidation;
using Shared.Results.Errors;
using Shared.Results.Errors.Auth;

namespace Application.Features.Auth.Logout;

public class LogoutRequestValidator : AbstractValidator<LogoutRequest>
{
    public LogoutRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithState(_ => new ValidationError(
                Code: AuthError.TokenRequired.Code,
                Message: AuthError.TokenRequired.Message,
                PropertyName: nameof(LogoutRequest.Token)
                ));


        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithState(_ => new ValidationError(
                Code: AuthError.RefreshTokenRequired.Code,
                Message: AuthError.RefreshTokenRequired.Message,
                PropertyName: nameof(RefreshTokenRequest.RefreshToken)
                ));
    }
}
