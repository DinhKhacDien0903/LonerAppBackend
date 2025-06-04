using Loner.Application.DTOs;

namespace Loner.Application.Features.Setup
{
    public class SetUpUniversityHandler : IRequestHandler<SetUpUniversityRequest, Result<SetUpResponse>>
    {
        private readonly IUnitOfWork _uow;
        public SetUpUniversityHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<Result<SetUpResponse>> Handle(SetUpUniversityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.University))
                    return Result<SetUpResponse>.Failure("Invalid User or UserName");

                bool isSuccess = await _uow.UserRepository.UpdateUniversityByIdAsync(request.UserId, request.University);
                await _uow.CommitAsync();

                return isSuccess ? Result<SetUpResponse>.Success(new SetUpResponse(IsSuccess: true)) :
                                  Result<SetUpResponse>.Failure("Failed to update user name");
            }
            catch (Exception ex)
            {
                return Result<SetUpResponse>.Failure("Có lỗi khi thiết lập tài khoản " + ex.Message);
            }
        }
    }
}