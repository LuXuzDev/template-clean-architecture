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
                Code: UserError.EmailRequired.Code,
                Message: UserError.EmailRequired.Message,
                PropertyName: nameof(RegisterClientRequest.Email)
            ))
            .EmailAddress()
            .WithState(_ => new ValidationError(
                Code: UserError.InvalidEmailFormat.Code,
                Message: UserError.InvalidEmailFormat.Message,
                PropertyName: nameof(RegisterClientRequest.Email)
            ));


        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithState(_ => new ValidationError(
                Code: UserError.PasswordRequired.Code,
                Message: UserError.PasswordRequired.Message,
                PropertyName: nameof(RegisterClientRequest.Password)
            ))

            .Must(PasswordValidator.BeStrongPassword)
            .WithState(_ => new ValidationError(
                Code: UserError.WeakPassword.Code,
                Message: UserError.WeakPassword.Message,
                PropertyName: nameof(RegisterClientRequest.Password)
            ));
    }
}
