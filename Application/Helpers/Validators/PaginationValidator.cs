namespace Application.Helpers.Validators;

public static class PaginationValidator
{
    public static bool BeValidPageNumber(int pageNumber)
    => pageNumber >= 1;

    public static bool BeValidPageSize(int pageSize)
        => pageSize >= 1 && pageSize <= 100;
}
