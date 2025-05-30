namespace Loner.Application.DTOs;

public class UserIdentityDto
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? About { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Address { get; set; }
    public string? University { get; set; }
    public string? Work { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public bool Gender { get; set; }
    public int Age { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public IEnumerable<string>? Photos { get; set; }
    public IEnumerable<string>? Interests { get; set; }
}