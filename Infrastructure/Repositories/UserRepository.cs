using Loner.Domain.Common;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(LonerDbContext context) : base(context)
    {
    }

    public async Task<PaginatedResponse<UserEntity>> GetAllUserByFilterAsync
        (string currentUserId, string? userName = null, string? phoneNumber = null, string? email = null, int pageNumber = 1, int pageSize = 30)
    {
        pageNumber = pageNumber + 1;
        PaginatedResponse<UserEntity> result = new();
        var query = from u in _context.Users
                    where u.Id != currentUserId
                    && (string.IsNullOrEmpty(userName) || (u.UserName ?? "").Trim().ToLower().Contains(userName.Trim().ToLower()))
                    && (string.IsNullOrEmpty(phoneNumber) || (u.PhoneNumber ?? "").ToLower().Trim().Contains(phoneNumber.ToLower()))
                    && (string.IsNullOrEmpty(email) || (u.Email ?? "").Trim().ToLower().Contains(email.ToLower()))
                    select u;

        result.TotalItems = await query.CountAsync();
        result.PageSize = pageSize;
        result.Items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return result;
    }

    public async Task<int> GetTotalUserCountAsync()
    {
        return await _context.Users.CountAsync();
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => !x.IsDeleted && x.Email == email);
    }

    public async Task<IEnumerable<UserEntity?>> GetUserByNameAsync(string name)
    {
        return await _context.Users
            .Where(x => !x.IsDeleted && x.UserName.Trim().Contains(name.Trim(), StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
    }

    public async Task<UserEntity?> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Users.FirstOrDefaultAsync(x => !x.IsDeleted && x.PhoneNumber == phoneNumber);
    }

    public async Task<UserEntity?> GetUserContainNameAsync(string userId, string containValue)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId) && x.UserName.Trim().Contains(containValue));
    }

    public async Task<string?> GetUserNameByIdAsync(string userId)
    {
        return await _context.Users
            .Where(x => !x.IsDeleted && x.Id == userId)
            .Select(x => x.UserName ?? string.Empty)
            .FirstOrDefaultAsync();
    }

    public async  Task<bool> IsUserDeletedAsync(string userId)
    {
        return await _context.Users
            .Where(x => x.Id == userId)
            .Select(x => x.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateDeleteStatusAsync(string userId, bool isDeleted)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsDeleted = isDeleted;
                Update(user);

                return true;
            }

            return false;
        }
        catch (Exception)
        {
            return false;
        }
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

    public async Task<bool> UpdateGenderByIdAsync(string userId, bool gender)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.Gender = gender;
            Update(user);
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateDateOfBirthByIdAsync(string userId, DateTime dob)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.DateOfBirth = dob;
            Update(user);
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateUniversityByIdAsync(string userId, string university)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.University = university;
            Update(user);
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateUserNameByIdAsync(string userId, string newUserName)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.UserName = newUserName;
            Update(user);
            return true;
        }
        return false;
    }
}