using CleanArch.Application.Abstractions.Infrastructure.Services;
using CleanArch.Application.DTOs.Email;
using CleanArch.Application.Exceptions;
using CleanArch.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CleanArch.Infrastructure.Services
{
    public class MailService : IMailService
    {
        public MailSettings _mailSettings { get; }
        public ILogger<MailService> Logger { get; }

        public MailService(IOptions<MailSettings> mailSettings, ILogger<MailService> logger)
        {
            _mailSettings = mailSettings.Value;
            Logger = logger;
        }

        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new(request.From ?? _mailSettings.EmailFrom, "CleanArchitecture", System.Text.Encoding.UTF8);
                foreach (var to in request.To)
                    mail.To.Add(to);
                mail.Subject = request.Subject;
                mail.Body = request.Body;
                mail.IsBodyHtml = true;


                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                smtp.Port = _mailSettings.SmtpPort;
                smtp.Host = _mailSettings.SmtpHost;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mail);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            try
            {
                StringBuilder mail = new();
                mail.AppendLine("Merhaba<br>Aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br><strong><a target=\"_blank\" href=\"");
                mail.AppendLine("URL");
                mail.AppendLine("/update-password/");
                mail.AppendLine(userId);
                mail.AppendLine("/");
                mail.AppendLine(resetToken);
                mail.AppendLine("\">Yeni şifre talebi için tıklayınız...</a></strong><br><br><span style=\"font-size:12px;\"></span><br>Saygılarımızla.<br><br><br>CleanArchitecture");

                await SendAsync(new EmailRequest() { Subject = "Şifre Yenileme Talebi", Body = mail.ToString(), To = new List<string>() { to } });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }

        }
    }
}