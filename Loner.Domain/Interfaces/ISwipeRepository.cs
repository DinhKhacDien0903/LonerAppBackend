using Loner.Domain.Common;

namespace Loner.Domain.Interfaces;

public interface ISwipeRepository : IBaseRepository<SwipeEntity>
{
    Task<SwipeEntity?> GetSwipeAsync(string swiperId, string swipedId);
    Task<PaginatedResponse<UserEntity>> GetUnSwipedUsersAsync(string userId, int pageNumber, int pageSize = 30);
    Task<IEnumerable<MatchesEntity>> GetMatchesAsync(string userId);
}