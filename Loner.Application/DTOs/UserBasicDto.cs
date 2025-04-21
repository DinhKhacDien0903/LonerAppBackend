namespace Loner.Application.DTOs;

public class UserBasicDto
{
    public string Id { get; set; } = string.Empty;
    public string? Username { get; set; }
    public int? Age { get; set; }
    public string AvatarUrl { get; set; } = string.Empty;
    public IEnumerable<string>? Interests { get; set; }
}