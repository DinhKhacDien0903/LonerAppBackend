using Loner.Application.DTOs;
using Loner.Domain;

namespace Loner.Application.Features.User
{
    public class UpdateResfreshTokenHandler : IRequestHandler<UpdateResfreshTokenRequest, Result<UpdateResfreshTokenResponse>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IJWTTokenService _jwtToken;
        public UpdateResfreshTokenHandler(
            IUnitOfWork uow,
            IJWTTokenService jWTTokenService)
        {
            _uow = uow;
            _jwtToken = jWTTokenService;
        }

        public async Task<Result<UpdateResfreshTokenResponse>> Handle(UpdateResfreshTokenRequest request, CancellationToken cancellationToken)
        {
            var validateResult = ValidateRequest(request);
            if (!validateResult.IsSuccess)
                return validateResult;

            var currentUser = await _uow.UserRepository.GetByIdAsync(request.UserId ?? "");
            var accessToken = await _jwtToken.GenerateJwtAccessToken(currentUser);
            var refreshToken = await _jwtToken.GenerateJwtRefreshToken(currentUser);

            await _uow.RefreshTokenRepository.AddAsync(CreateRefreshToken(currentUser.Id, refreshToken));
            return Result<UpdateResfreshTokenResponse>.Success(new UpdateResfreshTokenResponse(accessToken, refreshToken));
        }

        private Result<UpdateResfreshTokenResponse> ValidateRequest(UpdateResfreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId))
                return Result<UpdateResfreshTokenResponse>.Failure("User not null");
            if (string.IsNullOrEmpty(request.RefreshToken))
                return Result<UpdateResfreshTokenResponse>.Failure("Invalid RefreshToken");
            return Result<UpdateResfreshTokenResponse>.Success(null);
        }

        private RefreshTokenEntity CreateRefreshToken(string userId, string token)
        {
            return new RefreshTokenEntity
            {
                RefreshTokenID = Guid.NewGuid(),
                UserID = userId,
                Token = token,
                ExpiredAt = DateTime.UtcNow.AddDays(30),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}