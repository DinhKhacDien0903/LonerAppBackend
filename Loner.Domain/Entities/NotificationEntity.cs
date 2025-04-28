namespace Loner.Domain.Entities
{
    public class NotificationEntity : BaseEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public int Type { get; set; } // 0: like, 1:match, 2: message
        public string RelatedId { get; set; } = string.Empty; // UserId or MessageId or MatchId or SwipeId
        public string Content { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string? NotificationImage { get; set; }
        public bool IsRead { get; set; }
        [ForeignKey("ReceiverId")]
        public UserEntity? Receiver { get; set; }
    }
}