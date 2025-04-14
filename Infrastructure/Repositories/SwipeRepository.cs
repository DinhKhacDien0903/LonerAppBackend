using Loner.Domain.Entities;

namespace Infrastructure.Repositories;

public class SwipeRepository : BaseRepository<SwipeEntity>, ISwipeRepository
{
    public SwipeRepository(LonerDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MatchesEntity>> GetMatchesAsync(string userId)
    {
        return await _context.Matches.Where(x => x.User1Id == userId || x.User2Id == userId).ToListAsync();
    }

    public async Task<SwipeEntity?> GetSwipeAsync(string swiperId, string swipedId)
    {
        return await _context.Swipes.FirstOrDefaultAsync(x => x.SwiperId == swiperId && x.SwipedId == swipedId);
    }

    //Get 10 raw users until the user swiped
    public async Task<IEnumerable<UserEntity>> GetUnSwipedUsersAsync(string userId, int pageNumber, int pageSize)
    {
        var validPageNumber = Math.Max(1, pageNumber);
        var validPageSize = Math.Min(Math.Max(1, pageSize), 10);

        var swipedUsersIds = _context.Swipes
            .Where(x => x.SwiperId == userId)
            .Select(x => x.SwipedId);

        //todo: filter follow interest, preference
        var user = await _context.Users
            .Where(x => x.Id != userId && !swipedUsersIds.Contains(x.Id))
            .Skip((validPageNumber - 1) * validPageSize)
            .Take(validPageSize)
            .Select(x => new UserEntity
            {
                Id = x.Id,
                UserName = x.UserName,
                Age = x.Age,
                AvatarUrl = x.AvatarUrl,
            })
            .ToListAsync();

        return user;
    }
}