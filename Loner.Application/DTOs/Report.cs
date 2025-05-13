namespace Loner.Application.DTOs;

public class Report
{
    public record BlockRequest(string BlockerId, string BlockedId, byte TypeBlocked, bool IsUnChatBlocked) : IRequest<Result<BlockResponse>>;
    public record BlockResponse(bool IsSuccess);
    public record ReportRequest : IRequest<Result<ReportResponse>>
    {
        public ReportResponseDto Request { get; set; } = new ReportResponseDto();
    }
    public record ReportResponse(bool IsSuccess);

    public class ReportResponseDto
    {
        public string ReporterId { get; set; } = string.Empty;
        public string ReportedId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public byte TypeBlocked { get; set; } // 0: block profile, 1: block chat, 3: report
    }
}