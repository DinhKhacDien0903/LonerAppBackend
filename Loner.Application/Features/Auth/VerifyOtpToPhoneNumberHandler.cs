
using Loner.Domain;

namespace Loner.Application.Features.Auth;

public class VerifyOtpHandler : IRequestHandler<VerifyPhoneNumberRequest, Result<VerifyOtpResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly ISendOTPtoPhoneNumberService _generateOtpService;

    public VerifyOtpHandler(IUnitOfWork unitOfWork, ISendOTPtoPhoneNumberService generateOtpService)
    {
        _generateOtpService = generateOtpService;
        _uow = unitOfWork;
    }

    public async Task<Result<VerifyOtpResponse>> Handle(VerifyPhoneNumberRequest request, CancellationToken cancellationToken)
    {
        // var existsOtp = await _uow.OtpRepository.GetByPhoneNumberAsync(request.PhoneNumber);
        // if (existsOtp == null)
        //     return Result<VerifyOtpResponse>.Failure("Error: OTP not found");

        // if (existsOtp.Code != request.Otp || existsOtp.ExpiresAt < DateTime.UtcNow)
        // {
        //     return Result<VerifyOtpResponse>.Failure("Invalid or expired OTP");
        // }

        // await _uow.OtpRepository.DeleteAsync(existsOtp.Id);
        // await _uow.CommitAsync();
        await Task.Delay(1);
        if (!_generateOtpService.VeryfyOTPAsync(request.Otp))
            return Result<VerifyOtpResponse>.Failure("OTP is not correct or expired");

        var user = await _uow.UserRepository.GetUserByPhoneNumberAsync(request.PhoneNumber);
        //Handel if action is Logining
        if (request.IsLogining)
        {
            if (user == null)
                return Result<VerifyOtpResponse>.Failure("User not found");

            // Generate JWT token
            // var token = await _uow.TokenService.GenerateTokenAsync(user);
            return Result<VerifyOtpResponse>.Success(new VerifyOtpResponse(true));
        }

        if (user != null)
            return Result<VerifyOtpResponse>.Failure("User already exists");

        var newUser = new UserEntity
        {
            PhoneNumber = request.PhoneNumber
        };
        await _uow.UserRepository.AddAsync(newUser);
        await _uow.CommitAsync();

        // var token = _tokenService.GenerateToken(user);
        // var authResponse = _generateDtoService.ToAuthResponse(token);
        return Result<VerifyOtpResponse>.Success(new VerifyOtpResponse(true));
    }
}