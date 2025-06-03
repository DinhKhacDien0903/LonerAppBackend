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
    Task<bool> UpdateDeleteStatusAsync(string userId, bool isDeleted);
    Task<string?> GetUserNameByIdAsync(string userId);
    Task<bool> IsUserDeletedAsync(string userId);
    Task<bool> UpdateUserNameByIdAsync(string userId, string newUserName);
    Task<bool> UpdateDateOfBirthByIdAsync(string userId, DateTime dob);
    Task<bool> UpdateGenderByIdAsync(string userId, bool gender);
    Task<bool> UpdateUniversityByIdAsync(string userId, string university);
    Task<PaginatedResponse<UserEntity>> GetAllUserByFilterAsync(
        string currentUserId,
        string? userName = null,
        string? phoneNumber = null,
        string? email = null,
        int pageNumber = 1,
        int pageSize = 30
    );
}