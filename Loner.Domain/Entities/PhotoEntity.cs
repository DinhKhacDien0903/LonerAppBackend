namespace Loner.Domain.Entities
{
    public class PhotoEntity :BaseEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public UserEntity? User { get; set; }
    }
}