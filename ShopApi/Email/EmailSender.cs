using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ShopApi.Email
{
    public class EmailSender
    {
        private readonly EmailCredentials _emailCredentials;

        public EmailSender(IOptions<EmailCredentials> emailCredentials)
        {
            _emailCredentials = emailCredentials.Value;
        }

        public async Task<IdentityResult> SendMailAsync(string bodyMessage, string subject, string sendToUsername, string sendToEmail)
        {
            try
            {
                var emailInfo = new MimeMessage
                {
                    Subject = subject,
                    Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = bodyMessage }
                };

                emailInfo.From.Add(new MailboxAddress("Flaviu", _emailCredentials.Username + $"@gmail.com"));
                emailInfo.To.Add(new MailboxAddress(sendToUsername, sendToEmail));

                using var client = new SmtpClient();

                await client.ConnectAsync("smtp.gmail.com", 587);
                await client.AuthenticateAsync(_emailCredentials.Username, _emailCredentials.Password);
                await client.SendAsync(emailInfo);
                await client.DisconnectAsync(true);

                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
