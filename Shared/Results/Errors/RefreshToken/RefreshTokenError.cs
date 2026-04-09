namespace Shared.Results.Errors.RefreshToken;

public static class RefreshTokenError
{
    public static readonly Error NotFound =
    new("REFRESH_TOKEN_NOT_FOUND", "Refresh Token not found", 404);
}
