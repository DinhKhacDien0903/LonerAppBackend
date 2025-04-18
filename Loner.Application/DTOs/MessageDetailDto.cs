namespace Loner.Application.DTOs
{
    public class MessageDetailDto
    {
        public string Id { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsCurrentUserSend { get; set; }
        public bool IsImage { get; set; }
        public bool IsRead { get; set; }
        public DateTime? SendTime { get; set; }
    }
}