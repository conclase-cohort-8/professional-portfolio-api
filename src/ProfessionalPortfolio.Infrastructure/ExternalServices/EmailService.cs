using Mailjet.Client;
using Mailjet.Client.Resources;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json.Linq;
using ProfessionalPortfolio.Application.Commands;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.Settings;
using System.Runtime;

namespace ProfessionalPortfolio.Infrastructure.ExternalServices
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly IHostEnvironment _host;
        private readonly IMailjetClient _mailClient;

        public EmailService(IOptions<EmailSettings> options, IHostEnvironment host, IMailjetClient mailClient)
        {
            _settings = options.Value ??
                throw new ArgumentNullException("EmailSettings");
            _host = host;
            _mailClient = mailClient;
        }

        public async Task SendEmailAsync(EmailRequest emailRequest, string format = "plain")
        {
            var template = GetTemplate();
            if(!string.IsNullOrEmpty(template))
            {
                template = template.Replace("{{FirstName}}", emailRequest.To)
                    .Replace("{{OTP}}", 12345.ToString());
            }
            else
            {
                template = emailRequest.Body;
            }

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_settings.UserName));
            email.To.Add(MailboxAddress.Parse(emailRequest.To));
            email.Subject = emailRequest.Subject;
            email.Body = new TextPart(format) { Text = template };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.UserName, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task<bool> SendAsync(List<string> emails, string message, string subject)
        {
            List<MailjetResponse> responses = new List<MailjetResponse>();
            try
            {
                foreach (string mail in emails)
                {
                    MailjetRequest request = new MailjetRequest
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
                                    { "Email", "settings.Email" },
                                    { "Name", "settings.AppName" }
                                }
                            },
                            {
                                "To",
                                new JArray
                                {
                                    new JObject { { "Email", mail } }
                                }
                            },
                            { "Subject", subject },
                            { "TextPart", message },
                            { "HtmlPart", message },
                            { "CustomId", "settings.CustomId" }
                        }
                    });

                    responses.Add(await _mailClient.PostAsync(request));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return responses.All((MailjetResponse r) => r?.IsSuccessStatusCode ?? false);
        }

        public async Task<bool> SendAsync(string to, string message, string subject, IFormFile file)
        {
            using MemoryStream stream = new MemoryStream();
            file.CopyTo(stream);
            byte[] fileBytes = stream.ToArray();
            string base64String = Convert.ToBase64String(fileBytes);
            MailjetResponse response = null!;
            List<string> emails = new List<string> { to };
            try
            {
                foreach (string mail in emails)
                {
                    MailjetRequest request = new MailjetRequest
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
                                    { "Email", "settings.Email" },
                                    { "Name", "settings.AppName" }
                                }
                            },
                            {
                                "To",
                                new JArray
                                {
                                    new JObject { { "Email", mail } }
                                }
                            },
                            { "Subject", subject },
                            { "TextPart", message },
                            { "HtmlPart", message },
                            {
                                "Attachments",
                                new JArray
                                {
                                    new JObject
                                    {
                                        { "ContentType", "text/plain" },
                                        { "Filename", file.FileName },
                                        { "Base64Content", base64String }
                                    }
                                }
                            },
                            { "CustomId", "SwappaApp" }
                        }
                    });

                    response = await _mailClient.PostAsync(request);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return response?.IsSuccessStatusCode ?? false;
        }

        private string GetTemplate()
        {
            var path = Path.Combine(_host.ContentRootPath, "wwwroot", "templates", "account-verification.html");
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return string.Empty;
        }
    }
}