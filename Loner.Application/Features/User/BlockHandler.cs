using Loner.Domain.Entities;
using static Loner.Application.DTOs.Report;

namespace Loner.Application.Features.User;

public class BlockHandler : IRequestHandler<BlockRequest, Result<BlockResponse>>
{
    private readonly IUnitOfWork _uow;
    public BlockHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    public async Task<Result<BlockResponse>> Handle(BlockRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validateResult = ValidateRequest(request);
            if (!validateResult.IsSuccess)
                return validateResult;

            var block = new ReportEntity
            {
                Id = Guid.NewGuid().ToString(),
                ReporterId = request.BlockerId,
                ReportedId = request.BlockedId,
                TypeBlocked = request.TypeBlocked,
                Reason = "Block",
            };

            await _uow.ReportRepository.AddAsync(block);
            await _uow.CommitAsync();
            return Result<BlockResponse>.Success(new BlockResponse(IsSuccess: true));
        }
        catch (Exception ex)
        {
            return Result<BlockResponse>.Failure("Accoured an error: " + ex.Message);
        }
    }

    private Result<BlockResponse> ValidateRequest(BlockRequest request)
    {
        if (string.IsNullOrEmpty(request.BlockerId))
            return Result<BlockResponse>.Failure("Invalid BlockerId");
        if (string.IsNullOrEmpty(request.BlockedId))
            return Result<BlockResponse>.Failure("Invalid BlockedId");
        return Result<BlockResponse>.Success(null);
    }
}