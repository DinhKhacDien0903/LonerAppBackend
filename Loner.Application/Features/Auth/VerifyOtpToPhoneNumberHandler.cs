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
        // var existsOtp = await _uow.OtpRepository.GetByPhoneNumberAsync(request.PhoneNumber);
        // if (existsOtp == null)
        //     return Result<AuthResponse>.Failure("Error: OTP not found");

        // if (existsOtp.Code != request.Otp || existsOtp.ExpiresAt < DateTime.UtcNow)
        // {
        //     return Result<AuthResponse>.Failure("Invalid or expired OTP");
        // }

        // await _uow.OtpRepository.DeleteAsync(existsOtp.Id);
        // await _uow.CommitAsync();
        await Task.Delay(1);
        if (!_generateOtpService.VerifyOTPAsync(request.Otp))
            return Result<AuthResponse>.Failure("OTP is not correct or expired");

        var user = await _uow.UserRepository.GetUserByPhoneNumberAsync(request.PhoneNumber);
        AuthResponse tokenResponse;
        if (request.IsLogining)
        {
            if (user == null)
                return Result<AuthResponse>.Failure("User not found");

            tokenResponse = await TokenResponseAsync(user);
            return Result<AuthResponse>.Success(tokenResponse);
        }

        if (user != null)
            return Result<AuthResponse>.Failure("User already exists");

        var newUser = new UserEntity
        {
            Id = Guid.NewGuid().ToString(),
            PhoneNumber = request.PhoneNumber,
            IsVerifyAccount = true,
            TwoFactorEnabled = true
        };
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