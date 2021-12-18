using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _options;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<EmailSettings> options, ILogger<EmailSender> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
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

                _logger.LogInformation($"{email}: Email sent");
            }
            catch (Exception e)
            {
                _logger.LogError($"{email}: Email didn't send.\n Error: {e.Message}");
                throw;
            }
        }
    }
}
