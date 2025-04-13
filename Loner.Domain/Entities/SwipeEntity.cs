namespace Loner.Domain.Entities
{
    public class SwipeEntity : BaseEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string SwiperId { get; set; } = string.Empty;
        public string SwipedId { get; set; } = string.Empty;
        public bool Action { get; set; } // true = like, false = dislike
        [ForeignKey("SwiperId")]
        public UserEntity? Swiper { get; set; }
        [ForeignKey("SwipedId")]
        public UserEntity? Swiped { get; set; }
    }
}