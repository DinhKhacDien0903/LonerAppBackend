using Loner.Domain.Entities;

namespace Infrastructure.Repositories;

public class PhotoRepository : BaseRepository<PhotoEntity>, IPhotoRepository
{
    public PhotoRepository(LonerDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PhotoEntity>> GetPhotosByUserIdAsync(string userId)
    {
        return await _context.Photos.Where(p => p.UserId == userId).ToListAsync();
    }
}