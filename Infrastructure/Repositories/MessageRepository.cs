using Loner.Domain.Entities;

namespace Infrastructure.Repositories;

public class MessageRepository(LonerDbContext context) : BaseRepository<MessageEntity>(context), IMessageRepository
{
    public async Task<MessageEntity?> GetLastMessageByMatchIdAsync(string matchId)
    {
        return await _context.Messages
            .Where(x => x.MatchId == matchId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    }
}