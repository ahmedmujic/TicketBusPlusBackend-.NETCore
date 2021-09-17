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
using NotificationService.Constants;
using NotificationService.Events;
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

       

        public async Task<bool> SendEmailAsync(string emailBody, string reciever, string subject)
        {          

            try
            {
                _mail.To.Add(new MailboxAddress("Reciever", reciever));
                _mail.Subject = subject;

                _mail.Body = new TextPart(TextFormat.Html) { Text = emailBody };
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync(_emailSettings.Value.From, Environment.GetEnvironmentVariable("email_password"));
                    await client.SendAsync(_mail);
                    await client.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(SendEmailAsync));
                return false;
            }
        }


        public async Task<bool> SendEmailAsync(BodyBuilder body, string reciever, string subject)
        {
            try
            {
                _mail.Body = body.ToMessageBody();

                _mail.To.Add(new MailboxAddress("Reciever", reciever));
                _mail.Subject = subject;

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync(_emailSettings.Value.From, Environment.GetEnvironmentVariable("email_password"));
                    await client.SendAsync(_mail);
                    await client.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(SendEmailAsync));
                return false;
            }
        }
    }
}
