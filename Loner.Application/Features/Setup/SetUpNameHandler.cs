using Loner.Application.DTOs;

namespace Loner.Application.Features.User
{
    public class SetUpNameHandler : IRequestHandler<SetUpNameRequest, Result<SetUpResponse>>
    {
        private readonly IUnitOfWork _uow;
        public SetUpNameHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<Result<SetUpResponse>> Handle(SetUpNameRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.UserName))
                    return Result<SetUpResponse>.Failure("Invalid User or UserName");

                bool isSuccess = await _uow.UserRepository.UpdateUserNameByIdAsync(request.UserId, request.UserName);
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