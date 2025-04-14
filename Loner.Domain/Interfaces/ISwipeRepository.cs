namespace Loner.Domain.Interfaces;

public interface ISwipeRepository : IBaseRepository<SwipeEntity>
{
    Task<SwipeEntity?> GetSwipeAsync(string swiperId, string swipedId);
    Task<IEnumerable<UserEntity>> GetUnswipedUsersAsync(string userId, int pageNumber, int pageSize);
    Task<IEnumerable<MatchesEntity>> GetMatchesAsync(string userId);
}