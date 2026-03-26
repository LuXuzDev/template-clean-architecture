using Application.Helpers.Validators;
using FluentValidation;
using Shared.Results.Errors;
using Shared.Results.Errors.Pagination;

namespace Application.Features.Users.GetAll;

public class GetAllUsersRequestValidator : AbstractValidator<GetAllUsersRequest>
{
    public GetAllUsersRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .Must(PaginationValidator.BeValidPageNumber)
            .WithState(_ => new ValidationError(
                Code: PaginationErrors.InvalidPageNumber.Code,
                Message: PaginationErrors.InvalidPageNumber.Message,
                PropertyName: "Page Number"
            ));

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithState(_ => new ValidationError(
                Code: PaginationErrors.PageSizeTooSmall.Code,
                Message: PaginationErrors.PageSizeTooSmall.Message,
                PropertyName: "Page Size"
            ))
            .LessThanOrEqualTo(100)
            .WithState(_ => new ValidationError(
                Code: PaginationErrors.PageSizeTooLarge.Code,
                Message: PaginationErrors.PageSizeTooLarge.Message,
                PropertyName: "Page Size"
            ));

        RuleFor(x => x.SortBy)
        .IsInEnum()
        .WithState(_ => new ValidationError(
            Code: PaginationErrors.InvalidSortBy.Code,
            Message: PaginationErrors.InvalidSortBy.Message,
            PropertyName: "SortBy"
        ));
    }
}