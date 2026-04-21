namespace Application.Helpers.Validators;

public static class PaginationValidator
{
    public static bool BeValidPageNumber(int pageNumber)
    => pageNumber >= 1;

    public static int BeValidPageSize(int pageSize)
    {
        if (pageSize > 0 && pageSize <= 100)
            return 0; // Válido

        if (pageSize > 100)
            return 1; // Muy grande

        return -1; // Muy pequeño
    }
}
