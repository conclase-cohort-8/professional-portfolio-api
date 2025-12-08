
namespace ProfessionalPortfolio.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendAsync(string recipient, string message, string subject);
        Task SendEmailAsync(string to, string subject, string body, string format = "plain");
    }
}
