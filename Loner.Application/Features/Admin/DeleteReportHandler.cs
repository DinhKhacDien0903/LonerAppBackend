using Loner.Application.DTOs;

namespace Loner.Application.Features.Admin
{
    public class DeleteReportHandler : IRequestHandler<DeleteReportRequest, Result<DeleteReportResponse>>
    {
        private readonly IUnitOfWork _uow;
        public DeleteReportHandler(
            IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<Result<DeleteReportResponse>> Handle(DeleteReportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ResolverId))
                    return Result<DeleteReportResponse>.Failure("Current user not found");
                if (string.IsNullOrEmpty(request.ReportId))
                    return Result<DeleteReportResponse>.Failure("Current report not found");

                bool isSuccess = await _uow.ReportRepository.DeleteReportAsync(resolverId: request.ResolverId, reportId: request.ReportId);
                await _uow.CommitAsync();

                return isSuccess ?
                    Result<DeleteReportResponse>.Success(new DeleteReportResponse("Report has update successfully.", true)) :
                    Result<DeleteReportResponse>.Success(new DeleteReportResponse("Please conntact administrator system!", false));
            }
            catch (Exception ex)
            {
                return Result<DeleteReportResponse>.Failure($"Error: {ex.Message}");
            }
        }
    }
}