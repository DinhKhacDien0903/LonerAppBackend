namespace Loner.Domain.Interfaces
{
    public interface INotificationRepository : IBaseRepository<NotificationEntity>
    {
        Task<int> GetTotalRecordByUserIdAsync(string userId);
        Task<bool> IsNotificationExist(string senderId, string reciverId, string relatedId);
        Task<IEnumerable<NotificationEntity>> GetByUserIdPaginatedAsync(string userId, int pageNumber, int pageSize);
    }
}