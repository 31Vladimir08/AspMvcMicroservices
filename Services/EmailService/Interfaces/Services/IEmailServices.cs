namespace EmailService.Interfaces.Services
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
