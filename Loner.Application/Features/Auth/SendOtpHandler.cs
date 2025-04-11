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

        await _generateOtpService.SendOTPAsync(request.PhoneNumber);
        return Result<SendOTPResponse>.Success(new SendOTPResponse("OTP sent successfully"));
    }
}