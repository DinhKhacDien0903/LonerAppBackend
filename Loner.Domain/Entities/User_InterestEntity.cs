namespace Loner.Domain.Entities
{
    public class User_InterestEntity : BaseEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string InterestId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public UserEntity? User { get; set; }
        [ForeignKey("InterestId")]
        public InterestEntity? Interest { get; set; }
    }
}