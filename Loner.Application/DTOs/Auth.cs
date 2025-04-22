namespace Loner.Application.DTOs;

public class Auth
{
    public record RegisterPhoneNumberRequest(string PhoneNumber) : IRequest<Result<SendOTPResponse>>;
    public record VerifyPhoneNumberRequest(string PhoneNumber, string Otp, bool IsLoggingIn) : IRequest<Result<AuthResponse>>;
    public record RegisterEmailRequest(string Email) : IRequest<Result<SendOTPResponse>>;
    public record VerifyEmailRequest(string Email, string Otp, bool IsLoggingIn) : IRequest<Result<AuthResponse>>;
    public record LogoutRequest(string UserId, string RefreshToken) : IRequest<Result<LogoutResponse>>;
    public record DeleteAccountRequest(string UserId, string RefreshToken) : IRequest<Result<DeleteAccountResponse>>;
    public record AuthResponse(string AccessToken, string RefreshToken, bool IsVerified, string UserId, bool IsSetupedAccount, bool IsAccountExisted);
    public record LogoutResponse(bool IsSuccess);
    public record DeleteAccountResponse(bool IsSuccess);
    public record SendOTPResponse(string Message, bool IsSuccess);
}