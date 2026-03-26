namespace Application.Features.Auth.Register.Client;

public class RegisterClientRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
