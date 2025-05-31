using Loner.Domain.Common;

namespace Loner.Domain.Interfaces;

public interface IReportRepository : IBaseRepository<ReportEntity>
{
    Task AddOrUpdateReportAsync(ReportEntity report);
    Task<ReportEntity?> GetByReporterIdAndReportedId(ReportEntity report);
    Task<bool> IsUnChatBlocked(string reporterId, string reportedId, byte TypeBlocked);
    Task<bool> DeleteReportAsync(string resolverId, string reportId);
    Task<PaginatedResponse<ReportEntity>> GetAllReportByFilterAsync(
        string currentUserId,
        string? reporterName = null,
        string? reportedName = null,
        string? reason = null,
        int pageNumber = 1,
        int pageSize = 30
    );
}