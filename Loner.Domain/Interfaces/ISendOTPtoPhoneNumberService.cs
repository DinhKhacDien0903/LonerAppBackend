namespace Loner.Domain.Services;

public interface ISendOTPtoPhoneNumberService
{
    Task SendOTPAsync(string phoneNumber);
    bool VeryfyOTPAsync(string otp);
}