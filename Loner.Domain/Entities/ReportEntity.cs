namespace Loner.Domain.Entities;

public class ReportEntity : BaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public string ReporterId { get; set; } = string.Empty;
    public string ReportedId { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string? ResolverId { get; set; }
    public byte TypeBlocked { get; set; } // 0: report, 1: block chat, 3: block profile
    public bool IsUnChatBlocked { get; set; }
    public DateTime? ResolvedAt { get; set; }
    [ForeignKey("ReporterId")]
    public UserEntity? Reporter { get; set; }
    [ForeignKey("ReportedId")]
    public UserEntity? Resolver { get; set; }
    [ForeignKey("ResolverId")]
    public UserEntity? Reported { get; set; }
}