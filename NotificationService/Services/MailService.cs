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

        public MailService(IOptions<EmailSettings> emailSettings, ILogger<MailService> logger)
        {
            _emailSettings = emailSettings;
            _logger = logger;           
        }
              

        public async Task<bool> SendEmailAsync(string emailBody, string reciever, string subject)
        {          

            try
            {
                var mail = new MimeMessage();
                mail.From.Add(new MailboxAddress(_emailSettings.Value.FromName, _emailSettings.Value.From));
                mail.To.Add(new MailboxAddress("Reciever", reciever));
                mail.Subject = subject;

                mail.Body = new TextPart(TextFormat.Html) { Text = emailBody };
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync(_emailSettings.Value.From, Environment.GetEnvironmentVariable("email_password"));
                    await client.SendAsync(mail);
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
                var mail = new MimeMessage();
                mail.From.Add(new MailboxAddress(_emailSettings.Value.FromName, _emailSettings.Value.From));
                mail.To.Add(new MailboxAddress("Reciever", reciever));
                mail.Subject = subject;
                mail.Body = body.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync(_emailSettings.Value.From, Environment.GetEnvironmentVariable("email_password"));
                    await client.SendAsync(mail);
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
