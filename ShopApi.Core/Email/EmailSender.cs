﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using ShopApi.Constants;
using System;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ShopApi.Core.Email
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
                var builder = new BodyBuilder();

                builder.TextBody = bodyMessage;

                var image = builder.LinkedResources.Add(@"..\ShopApi.Core\Email\cat.jpg");

                image.ContentId = MimeUtils.GenerateMessageId();

                // Set the html version of the message text
                builder.HtmlBody = string.Format(@"<p>Hey Ioana,
                                                    <br>
                <p> Tin sa ca maine s-ar putea sa ma trezesc mai tarziu, undeva pe la 10.A durat ceva sa ma prind cum sa trimit un mail din C#<br>             
                <br>
                <center><img src=""cid:{0}""></center>
                   Happy Shopping", image.ContentId);

                //emailInfo.Body = builder.ToMessageBody();

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
        public async Task<IdentityResult> SendRegistrationMailAsync(string token, string userFullName, string sendToUsername, string sendToEmail)
        {
            try
            {
                using var client = new SmtpClient();

                var builder = new BodyBuilder();


                //  var image = builder.LinkedResources.Add(@"..\ShopApi.Core\Email\register.jpg");

                //image.ContentId = MimeUtils.GenerateMessageId();

                //builder.HtmlBody = $@"<p style=""color: red;"">Hello , <br> <b> {userFullName} </b> </p>

                // <center><img src=""cid:{image.ContentId}""></center>
                //Please click on the link below for confirming your email http://localhost:4200/confirm-email?email={sendToEmail}&token={token} 
                //<p> Best Regards,<br>
                //ShopOnlineApp2021 </p> ";

                builder.HtmlBody = String.Format(EmailTemplates.RegisterTemplate, userFullName);
                var emailInfo = new MimeMessage { Subject = "Confirm Email Shop Assistant", Body = builder.ToMessageBody() };

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
