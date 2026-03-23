namespace Shared.Results.Errors.User;

public static class UserValidationErrors
{
    public static readonly ValidationError EmailRequired =
        new("EMAIL_REQUIRED", "Email is required", "Email");

    public static readonly ValidationError PasswordTooWeak =
        new("PASSWORD_TOO_WEAK", "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character", "Password");
}

