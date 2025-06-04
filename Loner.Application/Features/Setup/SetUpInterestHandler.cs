using Loner.Application.DTOs;
using Loner.Domain.Entities;

namespace Loner.Application.Features.Setup
{
    public class SetUpInterestHandler : IRequestHandler<SetUpInterestRequest, Result<SetUpResponse>>
    {
        private readonly IUnitOfWork _uow;
        public SetUpInterestHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<Result<SetUpResponse>> Handle(SetUpInterestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    return Result<SetUpResponse>.Failure("Invalid User or UserName");

                bool isSuccess = await UpdateUserInterestAsync(request.UserId, request.Interests);

                await _uow.CommitAsync();

                return isSuccess ? Result<SetUpResponse>.Success(new SetUpResponse(IsSuccess: true)) :
                                  Result<SetUpResponse>.Failure("Failed to update iterest");
            }
            catch (Exception ex)
            {
                return Result<SetUpResponse>.Failure("Có lỗi khi thiết lập tài khoản " + ex.Message);
            }
        }

        private async Task<bool> UpdateUserInterestAsync(string userId, IEnumerable<string> interests)
        {
            try
            {
                var userIterests = await _uow.InterestRepository.GetUserInterestsByUserIdAsync(userId);
                foreach (var item in userIterests)
                {
                    _uow.InterestRepository.DeleteUserInterest(item);
                }

                foreach (var interest in interests)
                {
                    var newUserInterest = new User_InterestEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = userId,
                        InterestId = await _uow.InterestRepository.GetIdByNameAsync(interest)
                    };

                    await _uow.InterestRepository.AddUserInterestAsync(newUserInterest);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}