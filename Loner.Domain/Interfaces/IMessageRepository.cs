namespace Loner.Domain.Interfaces;

public interface IMessageRepository : IBaseRepository<MessageEntity>
{
    Task<int> GetTotalRecordByMatchIdAsync(string matchId);
    Task<MessageEntity?> GetLastMessageByMatchIdAsync(string matchId);
    Task<IEnumerable<MessageEntity>> GetMessagesPaginatedByMatchIdAsync(string matchId, int pageNumber, int pageSize);
}