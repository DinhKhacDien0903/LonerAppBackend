
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

                tokenResponse = await TokenResponseAsync(user);
                await _uow.OtpRepository.DeleteAsync(existsOtp.Id);
                await _uow.CommitAsync();
                return Result<AuthResponse>.Success(tokenResponse);
            }

            if (user != null)
                return Result<AuthResponse>.Failure("User already exists");

            var newUser = new UserEntity
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                IsVerifyAccount = true,
                TwoFactorEnabled = true
            };

            await _uow.OtpRepository.DeleteAsync(existsOtp.Id);
            await _uow.UserRepository.AddAsync(newUser);
            await _uow.CommitAsync();

            tokenResponse = await TokenResponseAsync(newUser);

            return Result<AuthResponse>.Success(tokenResponse);
        }

        private async Task<AuthResponse> TokenResponseAsync(UserEntity user)
        {
            return new AuthResponse(
                await _jwtToken.GenerateJwtAccessToken(user),
                await _jwtToken.GenerateJwtRefreshToken(user),
                true);
        }
    }
}