using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using MailKit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using NotificationService.Helpers;
using IMailService = NotificationService.Interfaces.IMailService;

namespace NotificationService.Services
{
    public class MailService : IMailService
    {
        private readonly IOptions<EmailSettings> _emailSettings;
        private readonly ILogger<MailService> _logger;
        private readonly MimeMessage _mail;

        public MailService(IOptions<EmailSettings> emailSettings, ILogger<MailService> logger)
        {
            _emailSettings = emailSettings;
            _logger = logger;
            _mail = new MimeMessage();
            _mail.From.Add(new MailboxAddress(_emailSettings.Value.FromName, _emailSettings.Value.From));
        }
        public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHTML = false, string fullName = null)
        {
            _mail.To.Add(new MailboxAddress("Reciever", to));
            _mail.Subject = subject;


            if (isHTML)
                _mail.Body = new TextPart(TextFormat.Html) { Text = EmailHtml.EmailActivation(body) };
            else
                _mail.Body = new TextPart(TextFormat.Plain) { Text = body };


            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync(_emailSettings.Value.From, _emailSettings.Value.Password);
                    await client.SendAsync(_mail);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (SmtpException exception)
            {
                System.Console.WriteLine(exception.Message);
                return false;
            }
        }

    }
}
