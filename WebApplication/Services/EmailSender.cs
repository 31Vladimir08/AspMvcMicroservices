using System;
using System.Threading.Tasks;

using EventBus.Messages.Events.EmailEvents;

using MassTransit;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using WebApplication.Models.Settings;

namespace WebApplication.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _options;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(
            IOptions<EmailSettings> options,
            IPublishEndpoint publishEndpoint,
            ILogger<EmailSender> logger)
        {
            _options = options.Value;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var eventMessage = new EmailEvent()
                {
                    Subject = subject,
                    HtmlMessage = htmlMessage,
                    Email = email
                };

                await _publishEndpoint.Publish(eventMessage);

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
