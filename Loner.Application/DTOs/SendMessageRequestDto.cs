namespace Loner.Application.DTOs
{
    public class SendMessageRequestDto
    {
        public string SenderId { get; set; } = string.Empty;
        public string MatchId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsImage { get; set; }
        public DateTime? SendTime { get; set; } = DateTime.UtcNow;
    }
}