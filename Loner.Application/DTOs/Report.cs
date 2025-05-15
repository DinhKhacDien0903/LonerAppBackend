namespace Loner.Application.DTOs;

public class Report
{
    public record BlockRequest(string BlockerId, string BlockedId, byte TypeBlocked, bool IsUnChatBlocked) : IRequest<Result<BlockResponse>>;
    public record CheckBlockdRequest(string BlockerId, string BlockedId, byte TypeBlocked) : IRequest<Result<CheckBlockdResponse>>;
    public record BlockResponse(bool IsSuccess);
    public record CheckBlockdResponse(bool IsUnChatBlocked);
    public record ReportRequest : IRequest<Result<ReportResponse>>
    {
        public ReportRequestDto Request { get; set; } = new ReportRequestDto();
    }
    public record ReportResponse(bool IsSuccess);

    public class ReportRequestDto
    {
        public string ReporterId { get; set; } = string.Empty;
        public string ReportedId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string MoreInformation { get; set; } = string.Empty;
        public byte TypeBlocked { get; set; } // 0: block profile, 1: block chat, 2: report
    }
}