using Loner.Application.DTOs;
using Loner.Domain;

namespace Loner.Application.Features.Admin
{
    public class SetAccoutLockStateHandler : IRequestHandler<SetAccoutLockStateRequest, Result<SetAccoutLockStateResponse>>
    {
        private readonly IUnitOfWork _uow;
        public SetAccoutLockStateHandler(
            IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<Result<SetAccoutLockStateResponse>> Handle(SetAccoutLockStateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    return Result<SetAccoutLockStateResponse>.Failure("User is set action not found");

                var isSuccess = await _uow.UserRepository.UpdateDeleteStatusAsync(request.UserId, request.IsDeleted);
                await _uow.CommitAsync();
                string message = isSuccess ?
                    (request.IsDeleted ? "User account has been locked successfully." : "User account has been unlocked successfully.") :
                    "Failed to update user account status.";
                return isSuccess ? 
                    Result<SetAccoutLockStateResponse>.Success(new SetAccoutLockStateResponse(message, isSuccess)) :
                    Result<SetAccoutLockStateResponse>.Success(new SetAccoutLockStateResponse("Please conntact administrator system!", false));
            }
            catch (Exception ex)
            {
                return Result<SetAccoutLockStateResponse>.Failure($"Error: {ex.Message}");
            }
        }
    }
}