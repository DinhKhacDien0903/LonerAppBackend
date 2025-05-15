using static Loner.Application.DTOs.Report;

namespace Loner.Application.Features.User;

public class CheckBlockdHandler : IRequestHandler<CheckBlockdRequest, Result<CheckBlockdResponse>>
{
    private readonly IUnitOfWork _uow;
    public CheckBlockdHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    public async Task<Result<CheckBlockdResponse>> Handle(CheckBlockdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validateResult = ValidateRequest(request);
            if (!validateResult.IsSuccess)
                return validateResult;

            var result = await _uow.ReportRepository.IsUnChatBlocked(request.BlockerId, request.BlockedId, request.TypeBlocked);
            return Result<CheckBlockdResponse>.Success(new CheckBlockdResponse(IsUnChatBlocked: true));
        }
        catch (Exception ex)
        {
            return Result<CheckBlockdResponse>.Failure("Accoured an error: " + ex.Message);
        }
    }

    private Result<CheckBlockdResponse> ValidateRequest(CheckBlockdRequest request)
    {
        if (string.IsNullOrEmpty(request.BlockerId))
            return Result<CheckBlockdResponse>.Failure("Invalid BlockerId");
        if (string.IsNullOrEmpty(request.BlockedId))
            return Result<CheckBlockdResponse>.Failure("Invalid BlockedId");
        return Result<CheckBlockdResponse>.Success(null);
    }
}