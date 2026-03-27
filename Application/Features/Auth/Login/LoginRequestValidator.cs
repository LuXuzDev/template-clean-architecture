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
                Code: AuthErrors.EmailRequired.Code,
                Message: AuthErrors.EmailRequired.Message,
                PropertyName: nameof(LoginRequest.Email)
            ))
            .EmailAddress()
            .WithState(_ => new ValidationError(
                Code: AuthErrors.InvalidEmailFormat.Code,
                Message: AuthErrors.InvalidEmailFormat.Message,
                PropertyName: nameof(LoginRequest.Email)
            ));


        RuleFor(x => x.Password)
            .NotEmpty()
            .WithState(_ => new ValidationError(
                Code: AuthErrors.PasswordRequired.Code,
                Message: AuthErrors.PasswordRequired.Message,
                PropertyName: nameof(LoginRequest.Password)
            ));
    }
}
