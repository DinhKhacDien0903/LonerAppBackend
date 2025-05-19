using Loner.Domain.Entities;

namespace Infrastructure.Repositories;

public class MessageRepository(LonerDbContext context) : BaseRepository<MessageEntity>(context), IMessageRepository
{
    private const int DEFAULT_PAGE_SIZE = 30;
    public async Task<MessageEntity?> GetLastMessageByMatchIdAsync(string matchId)
    {
        return await _context.Message
            .Where(x => x.MatchId == matchId && !x.IsMessageOfChatBot)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<MessageEntity>> GetMessagesPaginatedByMatchIdAsync
        (string matchId, int pageNumber, int pageSize = DEFAULT_PAGE_SIZE)
    {
        var validPageNumber = Math.Max(1, pageNumber);
        return await _context.Message
            .Where(x => x.MatchId == matchId && !x.IsMessageOfChatBot)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((validPageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalRecordByMatchIdAsync(string matchId)
    {
        return await  _context.Message.Where(x => x.MatchId == matchId && !x.IsMessageOfChatBot).CountAsync();
    }
}