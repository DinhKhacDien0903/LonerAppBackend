using Loner.Domain.Entities;

namespace Loner.Domain.Interfaces;

public interface IMatchesRepository : IBaseRepository<MatchesEntity>
{
    Task<MatchesEntity?> GetMatchAsync(string user1Id, string user2Id);
    Task<IEnumerable<MatchesEntity>> GetMatchPagiatedAsync(string userId, int pageNumber, int pageSize);
}