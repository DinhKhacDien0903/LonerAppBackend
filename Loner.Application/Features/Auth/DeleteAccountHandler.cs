
namespace Loner.Application.Features.Auth
{
    public class DeleteAccountHandler : IRequestHandler<DeleteAccountRequest, Result<DeleteAccountResponse>>
    {
        private readonly IUnitOfWork _uow;
        public DeleteAccountHandler(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<Result<DeleteAccountResponse>> Handle(DeleteAccountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var refreshToken = await _uow.RefreshTokenRepository.GetByTokenAsync(request.RefreshToken);
                var user = await _uow.UserRepository.GetByIdAsync(request.UserId);
                if (refreshToken == null || user == null)
                    return Result<DeleteAccountResponse>.Failure("Refresh token not found");

                user.IsActive = false;
                user.IsDeleted = true;
                refreshToken.IsUsed = true;
                refreshToken.IsRevoked = true;
                _uow.RefreshTokenRepository.Update(refreshToken);
                _uow.UserRepository.Update(user);
                await _uow.CommitAsync();

                return Result<DeleteAccountResponse>.Success(new DeleteAccountResponse(true));
            }
            catch (Exception ex)
            {
                return Result<DeleteAccountResponse>.Failure($"Error: {ex.Message}");
            }
        }
    }
}