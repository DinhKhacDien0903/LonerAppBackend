global using System.ComponentModel.DataAnnotations;
namespace Loner.Domain;

public class OTPEntity : BaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}