using Loner.Application.DTOs;

namespace Loner.Application.Features.Setup
{
    public class SetUpDOBHandler : IRequestHandler<SetUpDOBRequest, Result<SetUpResponse>>
    {
        private readonly IUnitOfWork _uow;
        public SetUpDOBHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<Result<SetUpResponse>> Handle(SetUpDOBRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    return Result<SetUpResponse>.Failure("Invalid User or UserName");

                bool isSuccess = await _uow.UserRepository.UpdateDateOfBirthByIdAsync(request.UserId, request.Dob);
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