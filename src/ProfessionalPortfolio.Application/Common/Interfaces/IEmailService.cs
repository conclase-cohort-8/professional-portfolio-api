using ProfessionalPortfolio.Application.Commands;

namespace ProfessionalPortfolio.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest emailRequest, string format = "plain");
    }
}