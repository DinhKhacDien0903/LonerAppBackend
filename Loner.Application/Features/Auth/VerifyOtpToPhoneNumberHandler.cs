
namespace Loner.Application.Features.Auth;

public class VerifyOtpHandler : IRequestHandler<VerifyPhoneNumberRequest, Result<VerifyOtpResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISendOTPtoPhoneNumberService _generateOtpService;

    public VerifyOtpHandler(IUnitOfWork unitOfWork, ISendOTPtoPhoneNumberService generateOtpService)
    {
        _generateOtpService = generateOtpService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<VerifyOtpResponse>> Handle(VerifyPhoneNumberRequest request, CancellationToken cancellationToken)
    {
        // var existsOtp = await _unitOfWork.OtpRepository.GetByPhoneNumberAsync(request.PhoneNumber);
        // if (existsOtp == null)
        //     return Result<VerifyOtpResponse>.Failure("Error: OTP not found");

        // if (existsOtp.Code != request.Otp || existsOtp.ExpiresAt < DateTime.UtcNow)
        // {
        //     return Result<VerifyOtpResponse>.Failure("Invalid or expired OTP");
        // }

        // await _unitOfWork.OtpRepository.DeleteAsync(existsOtp.Id);
        // await _unitOfWork.CommitAsync();
        await Task.Delay(1);
        if(_generateOtpService.VeryfyOTPAsync(request.Otp))
                return Result<VerifyOtpResponse>.Success(new VerifyOtpResponse(true));
        return Result<VerifyOtpResponse>.Failure("OTP is not correct");
    }
}