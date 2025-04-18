using Loner.Domain.Entities;

namespace Infrastructure.Repositories;

public class InterestRepository : BaseRepository<InterestEntity>, IInterestRepository
{
    public InterestRepository(LonerDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<InterestEntity>> GetInterestsByUserIdAsync(string userId)
    {
        var interestIds = await _context.User_Interests.Where(x => x.UserId == userId).Select(x => x.InterestId).ToListAsync();
        return await _context.Interests.Where(x => interestIds.Contains(x.Id)).ToListAsync();
    }
}