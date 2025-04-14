using System;

namespace Loner.Application.DTOs;

public class UserIdentityDto
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? About { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool IsVerifyAccount { get; set; }
    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; }
    public IEnumerable<string>? Photos { get; set; }
    public IEnumerable<string>? Interests { get; set; }
}