using Loner.Domain.Common;
using Loner.Domain.Entities;

namespace Infrastructure.Repositories;

public class ReportRepository : BaseRepository<ReportEntity>, IReportRepository
{
    public ReportRepository(LonerDbContext context) : base(context)
    {
    }

    public async Task AddOrUpdateReportAsync(ReportEntity report)
    {
        var existingReport = await GetByReporterIdAndReportedId(report);

        if (existingReport != null)
        {
            existingReport.IsUnChatBlocked = report.IsUnChatBlocked;
            Update(existingReport);
        }
        else
        {
            await AddAsync(report);
        }
    }

    public async Task<bool> DeleteReportAsync(string resolverId, string reportId)
    {
        var report = await GetByIdAsync(reportId);
        if (report == null)
            return false;

        report.ResolverId = resolverId;
        report.ResolvedAt = DateTime.UtcNow;
        Update(report);

        return true;
    }

    public async Task<PaginatedResponse<ReportEntity>> GetAllReportByFilterAsync
        (string currentUserId, string? reporterName = null, string? reportedName = null, string? reason = null, int pageNumber = 1, int pageSize = 30)
    {
        pageNumber = pageNumber == 0 ? pageNumber + 1 : pageNumber;
        reporterName = (reporterName ?? "").Trim().ToLower();
        reportedName = (reportedName ?? "").Trim().ToLower();
        reason = (reason ?? "").Trim().ToLower();
        PaginatedResponse<ReportEntity> result = new();
        var query = from r in _context.Report
                    join reporter in _context.Users on r.ReporterId equals reporter.Id
                    join reported in _context.Users on r.ReportedId equals reported.Id
                    where r.TypeBlocked == 2 
                    && string.IsNullOrEmpty(r.ResolverId)
                    && reporter.Id != currentUserId 
                    && reported.Id != currentUserId
                    && (string.IsNullOrEmpty(reporterName) || (reporter.UserName ?? "").ToLower().Trim().Contains(reporterName))
                    && (string.IsNullOrEmpty(reportedName) || (reported.UserName ?? "").ToLower().Trim().Contains(reportedName))
                    && (string.IsNullOrEmpty(reason) || (r.Reason ?? "").ToLower().Trim().Contains(reason))
                    select r;

        result.TotalItems = await query.CountAsync();
        result.PageSize = pageSize;
        result.Items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return result;
    }

    public async Task<ReportEntity?> GetByReporterIdAndReportedId(ReportEntity report)
    {
        return await _context.Report
            .FirstOrDefaultAsync(r => r.ReporterId == report.ReporterId && r.ReportedId == report.ReportedId);
    }

    public async Task<bool> IsUnChatBlocked(string reporterId, string reportedId, byte TypeBlocked)
    {
        return (await _context.Report
             .FirstOrDefaultAsync(r => r.ReporterId == reporterId && r.ReportedId == reportedId && r.TypeBlocked == TypeBlocked))?.IsUnChatBlocked ?? true;
    }
}