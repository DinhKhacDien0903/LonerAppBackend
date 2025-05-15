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