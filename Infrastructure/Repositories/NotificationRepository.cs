using Loner.Domain.Entities;

namespace Infrastructure.Repositories
{
    public class NotificationRepository(LonerDbContext context)
                : BaseRepository<NotificationEntity>(context), INotificationRepository
    {
        private const int DEFAULT_PAGE_SIZE = 30;
        public async Task<IEnumerable<NotificationEntity>> GetByUserIdPaginatedAsync
            (string userId, int pageNumber, int pageSize = DEFAULT_PAGE_SIZE)
        {
            var validPageNumber = Math.Max(1, pageNumber);
            return await _context.Notifications
                .Where(x => x.ReceiverId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Skip((validPageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalRecordByUserIdAsync(string userId)
        {
            return await _context.Notifications.Where(x => x.ReceiverId == userId).CountAsync();
        }

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