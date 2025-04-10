using Loner.Domain;

namespace Loner.Application.Interfaces;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<UserEntity> GetUserByEmailAsync(string email);
    Task<UserEntity> GetUserByPhoneNumberAsync(string phoneNumber);
}
