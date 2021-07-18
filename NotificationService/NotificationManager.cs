using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;
using Newtonsoft.Json.Linq;
using NotificationService.Constants;
using NotificationService.Events;
using NotificationService.Helpers;
using NotificationService.Interfaces;
using Serilog;

namespace NotificationService
{
    public class NotificationManager : IHostedService, IMessageHandlerCallback
    {
        IMessageHandler _messageHandler;
        private readonly IFileService _fileService;
        private readonly ILogger<NotificationManager> _logger;
        private readonly IMailService _mailService;

        public NotificationManager(IMessageHandler messageHandler,
            IMailService mailService,
            ILogger<NotificationManager> logger,
            IFileService fileService)
        {
            _logger = logger;
            _mailService = mailService;
            _messageHandler = messageHandler;
            _fileService = fileService;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Stop();
            return Task.CompletedTask;
        }

        public async Task<bool> HandleMessageAsync(string messageType, string message)
        {            
            try
            {
                JObject messageObject = MessageSerializer.Deserialize(message);
                switch (messageType)
                {
                    case MessageTypes.UserRegistered:
                        await HandleEmailAsync(messageObject.ToObject<UserRegistered>(), EmailConstants.UserActivation);
                        break;
                    case MessageTypes.ResetPassword:
                        await HandleEmailAsync(messageObject.ToObject<UserRegistered>(), EmailConstants.ResetPassword);
                        break;
                    case MessageTypes.InvoiceSend:
                        await HandleEmailAsync(messageObject.ToObject<InvoiceSend>(), EmailConstants.InvoiceSend);
                        break;
                    case MessageTypes.InfoSend:
                        await HandleEmailAsync(messageObject.ToObject<InfoSend>(), EmailConstants.InfoSend);
                        break;
                    default:
                        _logger.LogInformation(nameof(HandleMessageAsync) + "Invalid messageType");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while handling {messageType} event.");
            }

            return true;
        }

        private async Task HandleEmailAsync(UserRegistered ur, string subject)
        {
            try
            {
                  var emailHtmlBody =  subject switch
                    {
                        EmailConstants.UserActivation => EmailHtml.EmailActivation(ur.FirstName, ur.ConfrimationLink),
                        EmailConstants.ResetPassword => EmailHtml.ResetPassword(ur.FirstName, ur.ConfrimationLink),
                        EmailConstants.InfoSend => EmailHtml.InfoSend((InfoSend)ur),
                        _ => null,
                    };

                if(subject == EmailConstants.InvoiceSend)
                {
                    var attachment = _fileService.CreateInvoicePDF(EmailHtml.InvoiceHtml((InvoiceSend)ur));
                    BodyBuilder body = EmailHtml.InvoiceBody(attachment);
                    await _mailService.SendEmailAsync(body, ur.Email, subject);
                }
                else 
                 await _mailService.SendEmailAsync(emailHtmlBody, ur.Email, subject);
            }
            catch (Exception e)
            {
                _logger.LogError("Sending email exception: "+e.Message);
            }
        }

    }
}
