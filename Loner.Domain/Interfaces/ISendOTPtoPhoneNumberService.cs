namespace Loner.Domain.Services;

public interface ISendOTPtoPhoneNumberService
{
    Task SendOTPAsync(string phoneNumber);
    bool VerifyOTPAsync(string otp);
}