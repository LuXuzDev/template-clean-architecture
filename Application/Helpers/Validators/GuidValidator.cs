namespace Application.Helpers.Validators;

public static class GuidValidator
{
    public static bool BeValidGuid(string value)
    => Guid.TryParse(value, out _);
}
