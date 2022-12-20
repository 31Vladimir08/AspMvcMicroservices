using EmailService.Interfaces.Services;

using EventBus.Messages.Events.EmailEvents;

using MassTransit;

namespace EmailService.EventBusConsumer
{
    public class EmailConsumer : IConsumer<EmailEvent>
    {
        private readonly IEmailServices _emailServices;
        private readonly ILogger<EmailConsumer> _logger;

        public EmailConsumer(IEmailServices emailServices, ILogger<EmailConsumer> logger)
        {
            _emailServices = emailServices;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<EmailEvent> context)
        {
            try
            {
                await _emailServices.SendEmailAsync(context.Message.Email, context.Message.Subject, context.Message.HtmlMessage);
                _logger.LogInformation($"{context.Message.Email}: Email sent");
            }
            catch (Exception e)
            {
                _logger.LogError($"{context.Message.Email}: Email didn't send.\n Error: {e.Message}");
            }
            
        }
    }
}
