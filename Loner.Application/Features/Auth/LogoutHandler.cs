
using Loner.Application.Interfaces;

namespace Loner.Application.Features.Auth
{
    public class LogoutHandler : IRequestHandler<LogoutRequest, Result<LogoutResponse>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICookieService _cookieService;
        public LogoutHandler(IUnitOfWork unitOfWork, ICookieService cookieService)
        {
            _uow = unitOfWork;
            _cookieService = cookieService;
        }

        public async Task<Result<LogoutResponse>> Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var refreshToken = await _uow.RefreshTokenRepository.GetByTokenAsync(request.RefreshToken);
                var user = await _uow.UserRepository.GetByIdAsync(request.UserId);
                if (refreshToken == null || user == null)
                    return Result<LogoutResponse>.Failure("Refresh token not found");

                user.IsActive = false;
                refreshToken.IsUsed = true;
                refreshToken.IsRevoked = true;

                _cookieService.RemoveTokenToCookieHttpOnly("access_token");
                _cookieService.RemoveTokenToCookieHttpOnly("refresh_token");

                _uow.RefreshTokenRepository.Update(refreshToken);
                _uow.UserRepository.Update(user);
                await _uow.UserRepository.UpdateLastActiveAsync(user.Id);
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