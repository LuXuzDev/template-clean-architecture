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
                Code: UserErrors.InvalidGuidFormat.Code,
                Message: UserErrors.InvalidGuidFormat.Message,
                PropertyName: nameof(GenerateRefreshTokenRequest.UserId)
            ))

            .Must(GuidValidator.BeValidGuid)
            .WithState(_ => new ValidationError(
                Code: UserErrors.InvalidGuidFormat.Code,
                Message: UserErrors.InvalidGuidFormat.Message,
                PropertyName: nameof(GenerateRefreshTokenRequest.UserId)
            ));
    }
}
