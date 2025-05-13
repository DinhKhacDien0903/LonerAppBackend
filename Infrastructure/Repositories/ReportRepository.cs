using Loner.Domain.Entities;

namespace Infrastructure.Repositories;

public class ReportRepository : BaseRepository<ReportEntity>, IReportRepository
{
    public ReportRepository(LonerDbContext context) : base(context)
    {
    }
}