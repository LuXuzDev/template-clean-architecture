using System.Text.RegularExpressions;


namespace Application.Helpers.Validators;

public static class PasswordValidator
{
    public static bool BeStrongPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        return password.Length >= 8
            && Regex.IsMatch(password, "[A-Z]")
            && Regex.IsMatch(password, "[a-z]")
            && Regex.IsMatch(password, "[0-9]")
            && Regex.IsMatch(password, "[^a-zA-Z0-9]");
    }
}
