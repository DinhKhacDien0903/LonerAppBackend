namespace Loner.Domain.Entities
{
    public class MessageEntity : BaseEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string MatchId { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsMessageOfChatBot { get; set; }
        public bool IsImage { get; set; }
        [ForeignKey("MatchId")]
        public MatchesEntity? Matches { get; set; }
        [ForeignKey("SenderId")]
        public UserEntity? Sender { get; set; }
    }
}