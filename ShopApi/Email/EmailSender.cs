using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShopApi.Constants;

namespace ShopApi.Email
{
    public class EmailSender
    {
        private readonly EmailConfig _emailConfig;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<EmailConfig> emailConfig, ILogger<EmailSender> logger)
        {
            _emailConfig = emailConfig.Value;
            _logger = logger;
        }

        public async Task<IdentityResult> SendMailAsync(string bodyMessage, string subject, string sendToUsername, string sendToEmail)
        {
            try
            {
                using var client = new SmtpClient();
                var emailInfo = new MimeMessage
                {
                    Subject = subject,
                    Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = bodyMessage }
                };

                emailInfo.From.Add(new MailboxAddress(_emailConfig.Username, _emailConfig.Email));
                emailInfo.To.Add(new MailboxAddress(sendToUsername, sendToEmail));

                await client.ConnectAsync(_emailConfig.Host, 587);
                await client.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
                await client.SendAsync(emailInfo);
                await client.DisconnectAsync(true);

                _logger.LogInformation(string.Format(StringFormatTemplates.EmailSentSuccessfully, sendToEmail));

                return IdentityResult.Success;
            }
            catch (Exception e)
            {
               _logger.LogError(string.Format(StringFormatTemplates.EmailFailedToSend,sendToEmail) + e.Message );
                throw;
            }
        }
    }
}
