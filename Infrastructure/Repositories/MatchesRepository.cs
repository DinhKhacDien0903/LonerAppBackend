using Loner.Domain.Entities;

namespace Infrastructure.Repositories;

public class MatchesRepository : BaseRepository<MatchesEntity>, IMatchesRepository
{
    public MatchesRepository(LonerDbContext context) : base(context)
    {
    }

    public async Task<MatchesEntity?> GetMatchAsync(string user1Id, string user2Id)
    {
        return await _context.Matches
            .FirstOrDefaultAsync(x =>
                (x.User1Id == user1Id && x.User2Id == user2Id) ||
                (x.User1Id == user2Id && x.User2Id == user1Id));
    }
}