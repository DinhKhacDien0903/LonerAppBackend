namespace Loner.Application.DTOs
{
    public record GetNotificationsRequest(PaginationRequest PaginationRequest) : IRequest<Result<GetNotificationsResponse>>;
    public record ReadNotificationRequest(NotificationDto Notification) : IRequest<Result<UpdateNotificationResponse>>;
    public record RemoveNotificationRequest(NotificationDto Notification) : IRequest<Result<UpdateNotificationResponse>>;
    public record ClearNotificationRequest(string UserId) : IRequest<Result<ClearNotificationResponse>>;
    public record UpdateNotificationResponse(string Message, bool IsSuccess);
    public record ClearNotificationResponse(string Message, bool IsSuccess);
    public record GetNotificationsResponse(PaginatedResponse<NotificationDto> Notifications);
    public class NotificationDto
    {
        public string? Id { get; set; } = string.Empty;
        public string? Messeage { get; set; }
        public string ReceiverId { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public int Type { get; set; } = 0; // 0: like, 1:match, 2: message, 3: warning from admin
        public string RelatedId { get; set; } = string.Empty; // UserId or MessageId or MatchId or SwipeId
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string? NotificationImage { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
    }
}