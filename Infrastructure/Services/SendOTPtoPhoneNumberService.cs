using Loner.Domain.Services;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;

namespace Infrastructure.Services;

public class SendOTPtoPhoneNumberService : ISendOTPtoPhoneNumberService
{
    private readonly IConfiguration _configuration;
    public SendOTPtoPhoneNumberService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task SendOTPAsync(string phoneNumber)
    {
        try
        {
            InitTwilioClient();

            // var message = await MessageResource.CreateAsync(
            //     body: $"Your OTP is: {otp}",
            //     from: new Twilio.Types.PhoneNumber(fromPhoneNumber),
            //     to: new Twilio.Types.PhoneNumber(phoneNumber)
            // );

            var verification = await VerificationResource.CreateAsync(
                to: "+84777712640",
                channel: "sms",
                pathServiceSid: "VA197516f6d68a53f646a7274fd2f3cadd"
            );
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to send OTP", ex);
        }
    }

    public bool VeryfyOTPAsync(string otp)
    {
        InitTwilioClient();

        var verification = VerificationCheckResource.Create(
            to: "+84777712640",
            code: otp,
            pathServiceSid: _configuration["Twilio:PathServiceSid"]
        );

        return verification.Status == "approved";
    }

    private void InitTwilioClient()
    {
        var accountSid = _configuration["Twilio:AccountSid"];
        var authToken = _configuration["Twilio:AuthToken"];
        TwilioClient.Init(accountSid, authToken);
    }
}