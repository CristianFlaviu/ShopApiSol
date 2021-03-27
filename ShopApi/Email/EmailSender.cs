using Microsoft.AspNetCore.Identity;
using MimeKit;
using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit.Utils;
using ShopApi.Constants;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

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
                    // Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = bodyMessage }
                };
                var builder = new BodyBuilder();

                // Set the plain-text version of the message text
                builder.TextBody = bodyMessage;

                var image = builder.LinkedResources.Add(@"Email/cat.jpg");
                image.ContentId = MimeUtils.GenerateMessageId();

                // Set the html version of the message text
                builder.HtmlBody = string.Format(@"<p>Hey Ioana,
                                                    <br>
                <p> Tin sa ca maine s-ar putea sa ma trezesc mai tarziu, undeva pe la 10.A durat ceva sa ma prind cum sa trimit un mail din C#<br>             
                <br>
                <center><img src=""cid:{0}""></center>
                   Somn Usor", image.ContentId);

                emailInfo.Body = builder.ToMessageBody();

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
                _logger.LogError(string.Format(StringFormatTemplates.EmailFailedToSend, sendToEmail) + e.Message);
                throw;
            }
        }
    }
}
