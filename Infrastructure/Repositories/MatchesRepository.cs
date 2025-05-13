using Loner.Domain.Entities;

namespace Infrastructure.Repositories;

public class MatchesRepository : BaseRepository<MatchesEntity>, IMatchesRepository
{
    public MatchesRepository(LonerDbContext context) : base(context)
    {
    }

    public async Task<MatchesEntity?> GetMatchAsync(string user1Id, string user2Id)
    {
        return await _context.Matche
            .FirstOrDefaultAsync(x =>
                (x.User1Id == user1Id && x.User2Id == user2Id) ||
                (x.User1Id == user2Id && x.User2Id == user1Id));
    }

    public async Task<IEnumerable<MatchesEntity>> GetMatchPagiatedAsync(string userId, int pageNumber, int pageSize)
    {
        var validPageNumber = Math.Max(1, pageNumber);
        var validPageSize = Math.Min(Math.Max(1, pageSize), 10);

        var reportedIds = await _context.Report
            .Where(x => x.ReporterId == userId
                    && x.TypeBlocked == 1
                    && !x.IsUnChatBlocked
                    && !x.IsDeleted)
            .Select(x => x.ReportedId)
            .ToListAsync();

        var matches = _context.Matche
            .Where(x =>
            (x.User1Id == userId && !reportedIds.Contains(x.User2Id))
            || (x.User2Id == userId && !reportedIds.Contains(x.User1Id)));
        var totalItems = await matches.CountAsync();
        var paginatedMatches = await matches
            .Skip((validPageNumber - 1) * validPageSize)
            .Take(validPageSize)
            .ToListAsync();

        return paginatedMatches;
    }
}