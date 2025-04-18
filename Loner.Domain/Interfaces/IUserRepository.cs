namespace Loner.Domain.Interfaces;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<UserEntity?> GetUserByEmailAsync(string email);
    Task<UserEntity?> GetUserByPhoneNumberAsync(string phoneNumber);
    Task<int> GetTotalUserCountAsync();
    Task UpdateLastActiveAsync(string userId);
}