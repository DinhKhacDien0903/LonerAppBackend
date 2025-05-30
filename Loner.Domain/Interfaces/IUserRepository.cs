using Loner.Domain.Common;

namespace Loner.Domain.Interfaces;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<UserEntity?> GetUserByEmailAsync(string email);
    Task<UserEntity?> GetUserByPhoneNumberAsync(string phoneNumber);
    Task<IEnumerable<UserEntity?>> GetUserByNameAsync(string phoneNumber);
    Task<UserEntity?> GetUserContainNameAsync(string userId, string containValue);
    Task<int> GetTotalUserCountAsync();
    Task UpdateLastActiveAsync(string userId);
    Task<PaginatedResponse<UserEntity>> GetAllUserByFilterAsync(
        string currentUserId,
        string? userName = null,
        string? phoneNumber = null,
        string? email = null,
        int pageNumber = 1,
        int pageSize = 30
    );
}