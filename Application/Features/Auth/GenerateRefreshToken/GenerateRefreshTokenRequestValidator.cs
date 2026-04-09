using Application.Helpers.Validators;
using FluentValidation;
using Shared.Results.Errors;
using Shared.Results.Errors.User;

namespace Application.Features.Auth.GenerateRefreshToken;

public class GenerateRefreshTokenRequestValidator : AbstractValidator<GenerateRefreshTokenRequest>
{
    public GenerateRefreshTokenRequestValidator()
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithState(_ => new ValidationError(
                Code: UserError.InvalidGuidFormat.Code,
                Message: UserError.InvalidGuidFormat.Message,
                PropertyName: nameof(GenerateRefreshTokenRequest.UserId)
            ))

            .Must(GuidValidator.BeValidGuid)
            .WithState(_ => new ValidationError(
                Code: UserError.InvalidGuidFormat.Code,
                Message: UserError.InvalidGuidFormat.Message,
                PropertyName: nameof(GenerateRefreshTokenRequest.UserId)
            ));
    }
}
