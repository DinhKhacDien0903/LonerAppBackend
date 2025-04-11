
namespace Infrastructure.Repositories;

public class UserRepository :  BaseRepository<AppUser>, IUserRepository
{
    public UserRepository(LonerDbContext context) : base(context)
    {
    }

    public Task<UserEntity> AddAsync(UserEntity entity)
    {
        var appUser = AppUser.FromEntity(entity);
        return base.AddAsync(appUser).ContinueWith(t => AppUser.ToEntity(t.Result));
    }

    public Task<IEnumerable<UserEntity>> AddRangeAsync(IEnumerable<UserEntity> entities)
    {
        var appUsers = entities.Select(AppUser.FromEntity);
        return base.AddRangeAsync(appUsers).ContinueWith(t => t.Result.Select(AppUser.ToEntity));
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        var appUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        return appUser == null ? null : AppUser.ToEntity(appUser);
    }

    public async Task<UserEntity?> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        var appUser = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        return appUser == null ? null : AppUser.ToEntity(appUser);
    }

    public void Update(UserEntity entity)
    {
        var appUser = AppUser.FromEntity(entity);
        base.Update(appUser);
    }

    async Task<IEnumerable<UserEntity>> IBaseRepository<UserEntity>.GetAllAsync()
    {
        var appUsers = await base.GetAllAsync();
        return appUsers.Select(AppUser.ToEntity);
    }

    async Task<UserEntity?> IBaseRepository<UserEntity>.GetByIdAsync(string id)
    {
        var appUser = await base.GetByIdAsync(id);
        return appUser == null ? null : AppUser.ToEntity(appUser);
    }
}