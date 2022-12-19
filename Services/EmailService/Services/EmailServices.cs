using EmailService.Entities.Settings;
using EmailService.Interfaces.Services;

using MailKit.Net.Smtp;

using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailService.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly EmailSettings _options;

        public EmailServices(IOptions<EmailSettings> options)
        {
            _options = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", _options.Address));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_options.Host, _options.Port, true);
                await client.AuthenticateAsync(_options.UserName, _options.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
