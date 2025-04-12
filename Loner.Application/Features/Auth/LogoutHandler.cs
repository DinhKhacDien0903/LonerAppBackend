
namespace Loner.Application.Features.Auth
{
    public class LogoutHandler : IRequestHandler<LogoutRequest, Result<LogoutResponse>>
    {
        private readonly IUnitOfWork _uow;
        public LogoutHandler(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<Result<LogoutResponse>> Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var refreshToken = await _uow.RefreshTokenRepository.GetByTokenAsync(request.RefreshToken);
                if (refreshToken == null)
                    return Result<LogoutResponse>.Failure("Refresh token not found");

                refreshToken.IsUsed = true;
                refreshToken.IsRevoked = true;
                _uow.RefreshTokenRepository.Update(refreshToken);
                await _uow.CommitAsync();

                return Result<LogoutResponse>.Success(new LogoutResponse(true));
            }
            catch (Exception ex)
            {
                return Result<LogoutResponse>.Failure($"Error: {ex.Message}");
            }
        }
    }
}