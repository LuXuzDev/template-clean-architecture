using FluentValidation;
using Shared.Results.Errors;
using Shared.Results.Errors.Auth;


namespace Application.Features.Auth.Login;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithState(_ => new ValidationError(
                Code: AuthError.EmailRequired.Code,
                Message: AuthError.EmailRequired.Message,
                PropertyName: nameof(LoginRequest.Email)
            ))
            .EmailAddress()
            .WithState(_ => new ValidationError(
                Code: AuthError.InvalidEmailFormat.Code,
                Message: AuthError.InvalidEmailFormat.Message,
                PropertyName: nameof(LoginRequest.Email)
            ));


        RuleFor(x => x.Password)
            .NotEmpty()
            .WithState(_ => new ValidationError(
                Code: AuthError.PasswordRequired.Code,
                Message: AuthError.PasswordRequired.Message,
                PropertyName: nameof(LoginRequest.Password)
            ));
    }
}
