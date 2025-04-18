namespace Loner.Domain.Interfaces;

public interface IMessageRepository : IBaseRepository<MessageEntity>
{
    Task<MessageEntity?> GetLastMessageByMatchIdAsync(string matchId);
}