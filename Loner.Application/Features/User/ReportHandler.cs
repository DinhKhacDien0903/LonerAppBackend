using Loner.Domain.Entities;
using static Loner.Application.DTOs.Report;

namespace Loner.Application.Features.User;

public class ReportHandler : IRequestHandler<ReportRequest, Result<ReportResponse>>
{
    private readonly IUnitOfWork _uow;
    public ReportHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    public async Task<Result<ReportResponse>> Handle(ReportRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validateResult = ValidateRequest(request);
            if (!validateResult.IsSuccess)
                return validateResult;

            var block = new ReportEntity
            {
                Id = Guid.NewGuid().ToString(),
                ReporterId = request.Request.ReporterId,
                ReportedId = request.Request.ReportedId,
                TypeBlocked = request.Request.TypeBlocked,
                MoreInformation = request.Request.MoreInformation,
                Reason = request.Request.Reason,
            };

            await _uow.ReportRepository.AddAsync(block);
            await _uow.CommitAsync();
            return Result<ReportResponse>.Success(new ReportResponse(IsSuccess: true));
        }
        catch (Exception ex)
        {
            return Result<ReportResponse>.Failure("Accoured an error: " + ex.Message);
        }
    }

    private Result<ReportResponse> ValidateRequest(ReportRequest request)
    {
        if (string.IsNullOrEmpty(request.Request.ReporterId))
            return Result<ReportResponse>.Failure("Invalid BlockerId");
        if (string.IsNullOrEmpty(request.Request.ReportedId))
            return Result<ReportResponse>.Failure("Invalid BlockedId");
        if (string.IsNullOrEmpty(request.Request.Reason))
            return Result<ReportResponse>.Failure("Invalid Reason");
        return Result<ReportResponse>.Success(null);
    }
}