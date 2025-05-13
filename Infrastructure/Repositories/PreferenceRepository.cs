using Loner.Domain.Entities;

namespace Infrastructure.Repositories;

public class PreferenceRepository : BaseRepository<PreferenceEntity>, IPreferenceRepository
{
    public PreferenceRepository(LonerDbContext context) : base(context)
    {
    }

    public async Task<PreferenceEntity?> GetByUserId(string userId)
    {
        return await _context.Preference.Where(x => x.UserId.Equals(userId)).FirstOrDefaultAsync();
    }
}