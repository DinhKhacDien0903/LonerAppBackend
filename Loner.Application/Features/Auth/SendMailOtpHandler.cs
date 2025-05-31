
using Loner.Domain;

namespace Loner.Application.Features.Auth
{
    public class SendMailOtpHandler : IRequestHandler<RegisterEmailRequest, Result<SendOTPResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendOTPToMailService _generateOtpService;
        private readonly IUnitOfWork _uow;
        public SendMailOtpHandler(IUnitOfWork unitOfWork, ISendOTPToMailService generateDtoService)
        {
            _unitOfWork = unitOfWork;
            _generateOtpService = generateDtoService;
            _uow = unitOfWork;
        }

        public async Task<Result<SendOTPResponse>> Handle(RegisterEmailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var otp = GenerateOtp();

                var otpCode = new OTPEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = request.Email,
                    Code = otp,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(5)
                };

                await _unitOfWork.OtpRepository.AddAsync(otpCode);
                await _generateOtpService.SendOTPAsync(request.Email, otp);
                await _unitOfWork.CommitAsync();

                return Result<SendOTPResponse>.Success(new SendOTPResponse("OTP sent successfully", true));
            }
            catch (Exception ex)
            {
                return Result<SendOTPResponse>.Failure("Failed to send OTP: " + ex.Message);
            }
        }

        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}