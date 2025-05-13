using Loner.Domain.Entities;

namespace Infrastructure.Repositories
{
    public class NotificationRepository(LonerDbContext context)
                : BaseRepository<NotificationEntity>(context), INotificationRepository
    {
        private const int DEFAULT_PAGE_SIZE = 30;

        public async Task<bool> ClearNotificationsAsync(string userId)
        {
            var notifications = await GetAllAsync();
            if (notifications == null)
                return false;
            foreach(var item in notifications)
            {
                if (item.ReceiverId == userId && !item.IsDeleted)
                {
                    item.IsDeleted = true;
                    item.UpdatedAt = DateTime.UtcNow;
                    Update(item);
                }
            }

            return true;
        }

        public async Task<IEnumerable<NotificationEntity>> GetByUserIdPaginatedAsync
            (string userId, int pageNumber, int pageSize = DEFAULT_PAGE_SIZE)
        {
            var validPageNumber = Math.Max(1, pageNumber);
            return await _context.Notification
                .Where(x => x.ReceiverId == userId && !x.IsDeleted)
                .OrderBy(x => x.IsRead)
                .ThenByDescending(x => x.CreatedAt)
                .Skip((validPageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalRecordByUserIdAsync(string userId)
        {
            return await _context.Notification.Where(x => x.ReceiverId == userId).CountAsync();
        }

        public async Task<bool> IsNotificationExist(string senderId, string reciverId, string relatedId)
        {
            var notification = await _context.Notification
                .Where(n => n.ReceiverId == reciverId &&
                            (senderId != null || n.SenderId == senderId) &&
                            (n.RelatedId == relatedId) &&
                            !n.IsRead &&
                            !n.IsDeleted)
                .FirstOrDefaultAsync();

            return notification == null ? false : true;
        }

        public async Task<bool> UpdateIsDeleteNotificationAsync(string id, bool isDelete)
        {
            var notification = await GetByIdAsync(id);
            if (notification == null)
                return false;
            notification.IsDeleted = isDelete;
            notification.UpdatedAt = DateTime.UtcNow;
            Update(notification);

            return true;
        }

        public async Task<bool> UpdateIsReadNotificationAsync(string id, bool isRead)
        {
            var notification = await GetByIdAsync(id);
            if (notification == null)
                return false;
            notification.IsRead = isRead;
            notification.UpdatedAt = DateTime.UtcNow;
            Update(notification);

            return true;
        }
    }
}