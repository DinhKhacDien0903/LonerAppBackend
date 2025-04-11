namespace Loner.Application.Features.Auth;

public class SendOtpHandler : IRequestHandler<RegisterPhoneNumberRequest, Result<SendOTPResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISendOTPtoPhoneNumberService _generateOtpService;
    public SendOtpHandler(IUnitOfWork unitOfWork, ISendOTPtoPhoneNumberService generateDtoService)
    {
        _unitOfWork = unitOfWork;
        _generateOtpService = generateDtoService;
    }

    public async Task<Result<SendOTPResponse>> Handle(RegisterPhoneNumberRequest request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.UserRepository.GetUserByPhoneNumberAsync(request.PhoneNumber);
        if (existingUser != null)
            return Result<SendOTPResponse>.Failure("User with this phone number already exists");

        // var otp = GenerateOtp();

        // var otpCode = new OTPEntity
        // {
        //     Id = Guid.NewGuid().ToString(),
        //     PhoneNumber = request.PhoneNumber,
        //     Code = otp,
        //     ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        // };

        // var task1 = _unitOfWork.OtpRepository.AddAsync(otpCode);
        // var task2 = _generateOtpService.SendOTPAsync(request.PhoneNumber, otp);

        // await Task.WhenAll(task1, task2);
        // await _unitOfWork.CommitAsync();

        await _generateOtpService.SendOTPAsync(request.PhoneNumber);
        return Result<SendOTPResponse>.Success(new SendOTPResponse("OTP sent successfully"));
    }

    private string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}