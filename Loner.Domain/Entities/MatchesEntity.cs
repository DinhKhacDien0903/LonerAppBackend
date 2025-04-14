namespace Loner.Domain.Entities
{
    public class MatchesEntity : BaseEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string User1Id { get; set; } = string.Empty;
        public string User2Id { get; set; } = string.Empty;
        [ForeignKey("User1Id")]
        public UserEntity? User1 { get; set; }
        [ForeignKey("User2Id")]
        public UserEntity? User2 { get; set; }
    }
}