
namespace Infrastructure.Repositories;

public class UserRepository :  BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(LonerDbContext context) : base(context)
    {
    }

    public async Task<int> GetTotalUserCountAsync()
    {
       return await _context.Users.CountAsync();
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<UserEntity?> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }

    public async Task UpdateLastActiveAsync(string userId)
    {
        var user = await GetByIdAsync(userId);
        if(user != null)
        {
            user.LastActive = DateTime.UtcNow;
            Update(user);
        }
    }
}