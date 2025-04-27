namespace Loner.Domain.Interfaces
{
    public interface INotificationRepository : IBaseRepository<NotificationEntity>
    {
        Task<bool> IsNotificationExist(string senderId, string reciverId, string relatedId);
    }
}