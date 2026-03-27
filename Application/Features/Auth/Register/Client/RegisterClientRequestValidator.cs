using Application.Helpers.Validators;
using FluentValidation;
using Shared.Results.Errors;
using Shared.Results.Errors.User;

namespace Application.Features.Auth.Register.Client;

public class RegisterClientRequestValidator : AbstractValidator<RegisterClientRequest>
{
    public RegisterClientRequestValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithState(_ => new ValidationError(
                Code: UserErrors.EmailRequired.Code,
                Message: UserErrors.EmailRequired.Message,
                PropertyName: nameof(RegisterClientRequest.Email)
            ))
            .EmailAddress()
            .WithState(_ => new ValidationError(
                Code: UserErrors.InvalidEmailFormat.Code,
                Message: UserErrors.InvalidEmailFormat.Message,
                PropertyName: nameof(RegisterClientRequest.Email)
            ));


        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithState(_ => new ValidationError(
                Code: UserErrors.PasswordRequired.Code,
                Message: UserErrors.PasswordRequired.Message,
                PropertyName: nameof(RegisterClientRequest.Password)
            ))

            .Must(PasswordValidator.BeStrongPassword)
            .WithState(_ => new ValidationError(
                Code: UserErrors.WeakPassword.Code,
                Message: UserErrors.WeakPassword.Message,
                PropertyName: nameof(RegisterClientRequest.Password)
            ));
    }
}
