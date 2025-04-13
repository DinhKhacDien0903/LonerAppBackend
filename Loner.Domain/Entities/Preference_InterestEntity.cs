namespace Loner.Domain.Entities
{
    public class Preference_InterestEntity : BaseEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string PreferenceId { get; set; } = string.Empty;
        public string InterestId { get; set; } = string.Empty;
        [ForeignKey("PreferenceId")]
        public PreferenceEntity? Preference { get; set; }   
        [ForeignKey("InterestId")]
        public InterestEntity? Interest { get; set; }
    }
}