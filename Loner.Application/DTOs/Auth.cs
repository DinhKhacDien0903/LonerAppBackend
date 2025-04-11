using Loner.Application.Common;
using MediatR;

namespace Loner.Application.DTOs;

public class Auth
{
    public record RegisterPhoneNumberRequest(string PhoneNumber) : IRequest<Result<SendOTPResponse>>;
    public record VerifyPhoneNumberRequest(string PhoneNumber, string Otp, bool IsLogining) : IRequest<Result<AuthResponse>>;
    public record RegisterEmailRequest(string Email) : IRequest<Result<SendOTPResponse>>;
    public record AuthResponse(string AccessToken, string RefreshToken, bool IsVerified);
    public record SendOTPResponse(string Message);
}