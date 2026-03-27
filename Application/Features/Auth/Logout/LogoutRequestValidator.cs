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
                Code: AuthErrors.TokenRequired.Code,
                Message: AuthErrors.TokenRequired.Message,
                PropertyName: nameof(LogoutRequest.Token)
                ));


        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithState(_ => new ValidationError(
                Code: AuthErrors.RefreshTokenRequired.Code,
                Message: AuthErrors.RefreshTokenRequired.Message,
                PropertyName: nameof(RefreshTokenRequest.RefreshToken)
                ));
    }
}
