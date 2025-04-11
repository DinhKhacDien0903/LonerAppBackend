namespace Loner.Domain.Interfaces
{
    public interface ISendOTPToMailService
    {
        Task<bool> SendOTPAsync(string email, string otp);
        bool VerifyOTPAsync(string otp);
    }
}