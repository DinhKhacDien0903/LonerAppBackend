namespace Loner.Application.DTOs;

public class UserMessageBasicDto
{
    public string UserId { get; set; } = string.Empty;
    public string MatchId { get; set; } = string.Empty;
    public bool IsCurrentUserSend { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string? LastMessage { get; set; }
    public DateTime? SendTime { get; set; }
}