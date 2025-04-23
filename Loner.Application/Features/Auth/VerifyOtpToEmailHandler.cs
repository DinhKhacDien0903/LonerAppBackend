
using Loner.Domain;

namespace Loner.Application.Features.Auth
{
    public class VerifyOtpToEmailHandler : IRequestHandler<VerifyEmailRequest, Result<AuthResponse>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IJWTTokenService _jwtToken;

        public VerifyOtpToEmailHandler(
            IUnitOfWork unitOfWork,
            IJWTTokenService jwtToken)
        {
            _uow = unitOfWork;
            _jwtToken = jwtToken;
        }

        public async Task<Result<AuthResponse>> Handle(VerifyEmailRequest request, CancellationToken cancellationToken)
        {
            var existsOtp = await _uow.OtpRepository.GetByEmailAsync(request.Email);

            if (existsOtp == null)
                return Result<AuthResponse>.Failure("Error: OTP not found");

            if (existsOtp.Code != request.Otp || existsOtp.ExpiresAt < DateTime.UtcNow)
            {
                return Result<AuthResponse>.Failure("Invalid or expired OTP.");
            }

            var user = await _uow.UserRepository.GetUserByEmailAsync(request.Email);
            AuthResponse tokenResponse;
            if (request.IsLoggingIn)
            {
                if (user == null)
                    return Result<AuthResponse>.Failure("User not found");

                tokenResponse = await UpdateUserAndAddRefreshToken(user);
                return Result<AuthResponse>.Success(tokenResponse);
            }

            if (user != null)
                return Result<AuthResponse>.Failure("User already exists");

            var newUser = new UserEntity
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                IsVerifyAccount = true,
                TwoFactorEnabled = true,
                IsActive = true,
            };

            tokenResponse = await AddUserAndAddRefreshToken(newUser);
            return Result<AuthResponse>.Success(tokenResponse);
        }

        private async Task<AuthResponse> TokenResponseAsync(UserEntity user)
        {
            return new AuthResponse(
                await _jwtToken.GenerateJwtAccessToken(user),
                await _jwtToken.GenerateJwtRefreshToken(user),
                true,
                user.Id,
                await IsAccountSetup(user),
                user != null);
        }

        private async Task<bool> IsAccountExisted(string email)
        {
            var user = await _uow.UserRepository.GetUserByEmailAsync(email);
            return user != null;
        }

        private async Task<bool> IsAccountSetup(UserEntity user)
        {
            var photos = await _uow.PhotoRepository.GetPhotosByUserIdAsync(user.Id);
            var interests = await _uow.InterestRepository.GetInterestsByUserIdAsync(user.Id);
            return user?.DateOfBirth != null && !string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.University) && interests.Any() && photos.Any();
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

        private async Task<AuthResponse> UpdateUserAndAddRefreshToken(UserEntity user)
        {
            AuthResponse tokenResponse = await TokenResponseAsync(user);
            //await _uow.OtpRepository.DeleteAsync(existsOtp.Id);
            user.IsActive = true;
            _uow.UserRepository.Update(user);
            await _uow.RefreshTokenRepository.AddAsync(CreateRefreshToken(user.Id, tokenResponse.RefreshToken));
            await _uow.CommitAsync();

            return tokenResponse;
        }

        private async Task<AuthResponse> AddUserAndAddRefreshToken(UserEntity user)
        {
            AuthResponse tokenResponse = await TokenResponseAsync(user);
            //await _uow.OtpRepository.DeleteAsync(existsOtp.Id);
            await _uow.UserRepository.AddAsync(user);
            var refresh = CreateRefreshToken(user.Id, tokenResponse.RefreshToken);
            await _uow.RefreshTokenRepository.AddAsync(refresh);
            await _uow.CommitAsync();

            return tokenResponse;
        }
    }
}