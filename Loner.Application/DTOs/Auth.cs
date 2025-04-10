namespace Loner.Application.DTOs;

public class Auth
{
    public record RegisterPhoneNumberRequest(string PhoneNumber);
    public record RegisterEmailRequest(string Email);
    public record AuthResponse(string AccessToken, string RefreshToken);
}
