using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services
{
    public class SendOTPToMailService : ISendOTPToMailService
    {
        private readonly IConfiguration _configuration;
        public SendOTPToMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendOTPAsync(string toEmail, string otp)
        {
            string subject = "Mã xác thực OTP - Loner App";
            string htmlBody = GenerateOtpEmailMessage(toEmail, otp);

            try
            {
                var fromAddress = new MailAddress(_configuration["SendMail:FromAddress"] ?? "", "Loner App");
                var toAddress = new MailAddress(toEmail);
                string fromPassword = _configuration["SendMail:FromAddressPassword"] ?? "";

                var smtp = new SmtpClient
                {
                    Host = _configuration["SendMail:Host"] ?? "",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true
                };

                await smtp.SendMailAsync(message);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool VerifyOTPAsync(string otp)
        {
            throw new NotImplementedException();
        }

        private string GenerateOtpEmailMessage(string userName, string otpCode)
        {
            return $@"
                <html>
                    <head>
                        <style>
                            .otp-container {{
                                font-family: Arial, sans-serif;
                                max-width: 600px;
                                margin: auto;
                                border: 1px solid #eee;
                                padding: 20px;
                                border-radius: 10px;
                                background-color: #f9f9f9;
                            }}
                            .otp-code {{
                                font-size: 24px;
                                font-weight: bold;
                                color: #2c3e50;
                            }}
                            .footer {{
                                margin-top: 20px;
                                font-size: 12px;
                                color: #888;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='otp-container'>
                            <h2>Xác minh tài khoản Loner</h2>
                            <p>Xin chào <strong>{userName}</strong>,</p>
                            <p>Cảm ơn bạn đã sử dụng Loner. Mã xác thực OTP của bạn là:</p>
                            <p class='otp-code'>{otpCode}</p>
                            <p>Vui lòng nhập mã này vào ứng dụng để hoàn tất quá trình xác minh.</p>
                            <p class='footer'>Mã OTP sẽ hết hạn sau 5 phút. Nếu bạn không yêu cầu mã này, vui lòng bỏ qua email.</p>
                        </div>
                    </body>
                </html>";
        }
    }
}