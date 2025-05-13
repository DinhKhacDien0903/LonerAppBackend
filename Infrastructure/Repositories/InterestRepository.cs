using Loner.Domain.Entities;

namespace Infrastructure.Repositories;

public class InterestRepository : BaseRepository<InterestEntity>, IInterestRepository
{
    public InterestRepository(LonerDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<InterestEntity>> GetInterestsByUserIdAsync(string userId)
    {
        var interestIds = await _context.User_Interest.Where(x => x.UserId == userId).Select(x => x.InterestId).ToListAsync();
        return await _context.Interest.Where(x => interestIds.Contains(x.Id)).ToListAsync();
    }

    public void DeleteUserInterest(User_InterestEntity entity)
    {
        _context.User_Interest.Remove(entity);
    }

    public async Task<User_InterestEntity> AddUserInterestAsync(User_InterestEntity entity)
    {
        await _context.User_Interest.AddAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<User_InterestEntity>> GetUserInterestsByUserIdAsync(string userId)
    {
        return await _context.User_Interest.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<string> GetIdByNameAsync(string name)
    {
        return await _context.Interest.Where(x => x.Name == name).Select(x => x.Id).FirstOrDefaultAsync() ?? "";
    }
}