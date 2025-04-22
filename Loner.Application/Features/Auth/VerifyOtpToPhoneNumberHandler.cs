using Loner.Domain;

namespace Loner.Application.Features.Auth;

public class VerifyOtpOrRegisterHandler
    : IRequestHandler<VerifyPhoneNumberRequest, Result<AuthResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly ISendOTPtoPhoneNumberService _generateOtpService;
    private readonly IJWTTokenService _jwtToken;

    public VerifyOtpOrRegisterHandler(
        IUnitOfWork unitOfWork,
        ISendOTPtoPhoneNumberService generateOtpService,
        IJWTTokenService jwtToken)
    {
        _generateOtpService = generateOtpService;
        _uow = unitOfWork;
        _jwtToken = jwtToken;
    }

    public async Task<Result<AuthResponse>> Handle(VerifyPhoneNumberRequest request, CancellationToken cancellationToken)
    {
        if (!_generateOtpService.VerifyOTPAsync(request.Otp))
            return Result<AuthResponse>.Failure("OTP is not correct or expired");

        var user = await _uow.UserRepository.GetUserByPhoneNumberAsync(request.PhoneNumber);
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
            PhoneNumber = request.PhoneNumber,
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
            await IsAccountSetup(user?.Email ?? "", user?.Id ?? ""),
            await IsAccountExisted(user?.Email ?? ""));
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

    private async Task<bool> IsAccountExisted(string phoneNumber)
    {
        var user = await _uow.UserRepository.GetUserByPhoneNumberAsync(phoneNumber);
        return user != null;
    }

    private async Task<bool> IsAccountSetup(string phoneNumber, string userId)
    {
        var user = await _uow.UserRepository.GetUserByPhoneNumberAsync(phoneNumber);
        var photos = await _uow.PhotoRepository.GetPhotosByUserIdAsync(userId);
        var interests = await _uow.InterestRepository.GetInterestsByUserIdAsync(userId);
        return user?.DateOfBirth != null && user.UserName != null && user.University != null && interests.Any() && photos.Any();
    }

    private async Task<AuthResponse> UpdateUserAndAddRefreshToken(UserEntity user)
    {
        AuthResponse tokenResponse = await TokenResponseAsync(user);
        user.IsActive = true;
        _uow.UserRepository.Update(user);
        await _uow.RefreshTokenRepository.AddAsync(CreateRefreshToken(user.Id, tokenResponse.RefreshToken));
        await _uow.CommitAsync();

        return tokenResponse;
    }

    private async Task<AuthResponse> AddUserAndAddRefreshToken(UserEntity user)
    {
        AuthResponse tokenResponse = await TokenResponseAsync(user);
        await _uow.UserRepository.AddAsync(user);
        var refresh = CreateRefreshToken(user.Id, tokenResponse.RefreshToken);
        await _uow.RefreshTokenRepository.AddAsync(refresh);
        await _uow.CommitAsync();

        return tokenResponse;
    }
}