namespace Loner.Domain.Interfaces;

public interface IInterestRepository : IBaseRepository<InterestEntity>
{
    Task<IEnumerable<InterestEntity>> GetInterestsByUserIdAsync(string userId);
}
