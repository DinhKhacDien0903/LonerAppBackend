namespace Loner.Domain.Interfaces;

public interface IReportRepository : IBaseRepository<ReportEntity>
{
    Task AddOrUpdateReportAsync(ReportEntity report);
    Task<ReportEntity?> GetByReporterIdAndReportedId(ReportEntity report);
    Task<bool> IsUnChatBlocked(string reporterId, string reportedId, byte TypeBlocked);
}