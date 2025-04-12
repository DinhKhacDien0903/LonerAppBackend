using Loner.Domain.Entities;

namespace Infrastructure.Repositories;

public class OtpRepository : IOtpRepository
{
    private readonly LonerDbContext _context;

    public OtpRepository(LonerDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(OTPEntity otpCode)
    {
        await _context.Set<OTPEntity>().AddAsync(otpCode);
    }

    public async Task DeleteAsync(string id)
    {
        var otp = await GetOtpById(id);
        if (otp != null)
            _context.Set<OTPEntity>().Remove(otp);
    }

    public async Task<OTPEntity?> GetByEmailAsync(string email)
    {
        return await _context.Set<OTPEntity>().OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<OTPEntity?> GetOtpById(string id)
    {
        return await _context.Set<OTPEntity>().FindAsync(id);
    }

    public async Task<OTPEntity?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Set<OTPEntity>().FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }
}