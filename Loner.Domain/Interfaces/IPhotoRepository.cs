namespace Loner.Domain.Interfaces;

public interface IPhotoRepository : IBaseRepository<PhotoEntity>
{
    Task<IEnumerable<PhotoEntity>> GetPhotosByUserIdAsync(string userId);
}
