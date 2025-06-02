using Loner.Application.DTOs;
using Loner.Domain.Entities;

namespace Loner.Application.Features.Admin
{
    public class GetAllReportHandler : IRequestHandler<GetAllReportRequest, Result<GetAllReportResponse>>
    {
        private readonly IUnitOfWork _uow;
        public GetAllReportHandler(
            IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<Result<GetAllReportResponse>> Handle(GetAllReportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    return Result<GetAllReportResponse>.Failure("Current user not found");

                var reports = await _uow.ReportRepository.GetAllReportByFilterAsync(
                    currentUserId: request.UserId ?? "",
                    reporterName: request.ReporterName,
                    reportedName: request.ReportedName,
                    reason: request.Reason,
                    pageNumber: request.PageNumber,
                    pageSize: request.PageSize);

                PaginatedResponse<ReportDto> result = new();
                result.PageSize = reports.PageSize;
                result.TotalItems = reports.TotalItems;
                List<ReportDto> temps = new();
                foreach (var item in reports.Items)
                {
                    temps.Add((await MapReportDto(item)));
                }

                result.Items = temps.ToList();
                return Result<GetAllReportResponse>.Success(new GetAllReportResponse(result));
            }
            catch (Exception ex)
            {
                return Result<GetAllReportResponse>.Failure($"Error: {ex.Message}");
            }
        }

        private async Task<ReportDto> MapReportDto(ReportEntity report)
        {
            return new ReportDto
            {
                Id = report.Id,
                Reason = report.Reason,
                ReportedId = report.ReportedId,
                ReporterId = report.ReporterId,
                MoreInformation = report.MoreInformation,
                CreatedAt = report.CreatedAt,
                RepotedName = await _uow.UserRepository.GetUserNameByIdAsync(report.ReportedId),
                RepoterName = await _uow.UserRepository.GetUserNameByIdAsync(report.ReporterId),
                IsReportedBlocked = await _uow.UserRepository.IsUserDeletedAsync(report.ReportedId),
            };
        }
    }
}