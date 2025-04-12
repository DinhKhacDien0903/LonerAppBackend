namespace Loner.Domain.Interfaces;

public interface IOtpRepository
{
    Task AddAsync(OTPEntity otpCode);
    Task<OTPEntity?> GetByPhoneNumberAsync(string phoneNumber);
    Task<OTPEntity?> GetByEmailAsync(string email);
    Task DeleteAsync(string id);
}