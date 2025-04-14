
namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshTokenEntity>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(LonerDbContext context) : base(context)
        {
        }

        public async Task<RefreshTokenEntity?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token &&
                                                                        !x.IsUsed && !x.IsRevoked
                                                                        && x.ExpiredAt > DateTime.UtcNow);
        }
    }
}