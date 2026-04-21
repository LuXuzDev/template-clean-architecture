using Application.Helpers.Validators;
using FluentValidation;
using Shared.Results.Errors;
using Shared.Results.Errors.Pagination;

namespace Application.Features.Users.Admin.GetAll;

public class GetUsersRequestValidator : AbstractValidator<GetUsersRequest>
{
    public GetUsersRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .Must(PaginationValidator.BeValidPageNumber)
            .WithState(_ => new ValidationError(
                Code: PaginationError.InvalidPageNumber.Code,
                Message: PaginationError.InvalidPageNumber.Message,
                PropertyName: nameof(GetUsersRequest.PageNumber)
            ));

        RuleFor(x => x.PageSize)
            .Must(pageSize => PaginationValidator.BeValidPageSize(pageSize) == 0)
            .WithState(request => PaginationValidator.BeValidPageSize(request.PageSize) == -1
                ? new ValidationError(
                    Code: PaginationError.PageSizeTooSmall.Code,
                    Message: PaginationError.PageSizeTooSmall.Message,
                    PropertyName: nameof(GetUsersRequest.PageSize))
                : new ValidationError(
                    Code: PaginationError.PageSizeTooLarge.Code,
                    Message: PaginationError.PageSizeTooLarge.Message,
                    PropertyName: nameof(GetUsersRequest.PageSize)
            ));

        RuleFor(x => x.SortBy)
        .IsInEnum()
        .WithState(_ => new ValidationError(
            Code: PaginationError.InvalidSortBy.Code,
            Message: PaginationError.InvalidSortBy.Message,
            PropertyName: nameof(GetUsersRequest.SortBy)
        ));
    }
}