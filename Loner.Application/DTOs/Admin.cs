namespace Loner.Application.DTOs
{
    //Request
    public record GetAllUserForAdminRequest : IRequest<Result<GetAllUserForAdminResponse>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 30;
        public int ValidPageNumber => Math.Max(1, PageNumber);
        public int ValidPageSize => Math.Max(1, Math.Min(100, PageSize));
        public string UserId { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }

    public record GetAllReportRequest : IRequest<Result<GetAllReportResponse>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 30;
        public int ValidPageNumber => Math.Max(1, PageNumber);
        public int ValidPageSize => Math.Max(1, Math.Min(100, PageSize));
        public string UserId { get; set; } = string.Empty;
        public string? ReporterName { get; set; }
        public string? ReportedName { get; set; }
        public string? Reason { get; set; }
    }

    public record SetAccoutLockStateRequest : IRequest<Result<SetAccoutLockStateResponse>>
    {
        public string UserId { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = true;
    }

    public record DeleteReportRequest : IRequest<Result<DeleteReportResponse>>
    {
        public string ReportId { get; set; } = string.Empty;
        public string? ResolverId { get; set; }
    }

    //Response
    public record GetAllUserForAdminResponse(PaginatedResponse<UserIdentityDto> Data);
    public record GetAllReportResponse(PaginatedResponse<ReportDto> Data);
    public record DeleteReportResponse(string Message, bool IsSuccess);
    public class SetAccoutLockStateResponse
    {
        public SetAccoutLockStateResponse(string message, bool isSuccess)
        {
            Message = message;
            IsSuccess = isSuccess;
        }

        public string Message { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }
    }

    public class ReportDto
    {
        public string Id { get; set; } = string.Empty;
        public string ReporterId { get; set; } = string.Empty;
        public string ReportedId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string MoreInformation { get; set; } = string.Empty;
        public string? ResolverId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? RepoterName { get; set; }
        public string? RepotedName { get; set; }
    }
}