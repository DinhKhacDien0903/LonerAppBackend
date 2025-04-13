namespace Loner.Domain.Entities
{
    public class PreferenceEntity : BaseEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public bool PreferenceGender { get; set; }
        [ForeignKey("UserId")]
        public UserEntity? User { get; set; }
    }
}