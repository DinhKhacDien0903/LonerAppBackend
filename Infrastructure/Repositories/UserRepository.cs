
namespace Infrastructure.Repositories;

public class UserRepository :  BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(LonerDbContext context) : base(context)
    {
    }
    public async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<UserEntity?> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }
}