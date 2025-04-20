namespace Loner.Domain.Interfaces;

public interface IInterestRepository : IBaseRepository<InterestEntity>
{
    Task<IEnumerable<InterestEntity>> GetInterestsByUserIdAsync(string userId);
    Task<IEnumerable<User_InterestEntity>> GetUserInterestsByUserIdAsync(string userId);
    Task<User_InterestEntity> AddUserInterestAsync(User_InterestEntity entity);
    Task<string> GetIdByNameAsync(string name);
    void DeleteUserInterest(User_InterestEntity entity);
}