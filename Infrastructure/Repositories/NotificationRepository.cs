using Loner.Domain.Entities;

namespace Infrastructure.Repositories
{
    public class NotificationRepository(LonerDbContext context)
                : BaseRepository<NotificationEntity>(context), INotificationRepository
    {
        public async Task<bool> IsNotificationExist(string senderId, string reciverId, string relatedId)
        {
            var notification = await _context.Notifications
                .Where(n => n.ReceiverId == reciverId &&
                            (senderId != null || n.SenderId == senderId) &&
                            (n.RelatedId == relatedId) &&
                            !n.IsRead &&
                            !n.IsDeleted)
                .FirstOrDefaultAsync();

            return notification == null ? false : true;
        }
    }
}