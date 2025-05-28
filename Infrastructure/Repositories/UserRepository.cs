namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
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

    public async Task<IEnumerable<UserEntity?>> GetUserByNameAsync(string name)
    {
        return await _context.Users
            .Where(x => x.UserName.Trim().Contains(name.Trim(), StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
    }

    public async Task<UserEntity?> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }

    public async Task<UserEntity?> GetUserContainNameAsync(string userId, string containValue)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId) && x.UserName.Trim().Contains(containValue));
    }

    public async Task UpdateLastActiveAsync(string userId)
    {
        var user = await GetByIdAsync(userId);
        if (user != null)
        {
            user.LastActive = DateTime.UtcNow;
            Update(user);
        }
    }
}