using Mailjet.Client;
using Mailjet.Client.Resources;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json.Linq;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.Settings;

namespace ProfessionalPortfolio.Infrastructure.ExternalServices
{
    public class EmailService : IEmailService
    {
        private readonly MailKitSettings _mailSettings;
        private readonly MailJetSettings _jetSettings;
        private readonly IMailjetClient _mailjetClient;

        public EmailService(IOptions<MailKitSettings> options, 
            IOptions<MailJetSettings> jetOptions,
            IMailjetClient mailjetClient)
        {
            _mailSettings = options.Value ?? 
                throw new ArgumentNullException("MailKitSettings");
            _mailjetClient = mailjetClient;
            _jetSettings = jetOptions.Value ?? 
                throw new ArgumentNullException("MailJetSettings");
        }

        public async Task SendEmailAsync(string to, string subject, string body, string format = "plain")
        {
            //Prepare the message
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(_mailSettings.UserName));
            mail.To.Add(MailboxAddress.Parse(to));
            mail.Subject = subject;
            mail.Body = new TextPart(format)
            {
                Text = body
            };

            //Connection, authentication and sending
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
            await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);
        }

        public async Task<bool> SendAsync(string recipient, string message, string subject)
        {
            try
            {
                var request = new MailjetRequest
                {
                    Resource = SendV31.Resource
                }.Property("Messages", new JArray
                {
                    new JObject
                    {
                        {
                            "From",
                            new JObject
                            {
                                { "Email", _jetSettings.Email },
                                { "Name", _jetSettings.AppName }
                            }
                        },
                        {
                            "To",
                            new JArray
                            {
                                new JObject
                                {
                                    { "Email", recipient }
                                }
                            }
                        },
                        { "Subject", subject },
                        { "TextPart", message },
                        { "HtmlPart", message }
                    }
                });

                var response = await _mailjetClient.PostAsync(request);
                return response?.IsSuccessStatusCode ?? false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}