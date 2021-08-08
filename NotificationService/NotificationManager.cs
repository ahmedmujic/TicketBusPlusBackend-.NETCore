using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NotificationService.Constants;
using NotificationService.Events;
using NotificationService.Interfaces;
using Serilog;

namespace NotificationService
{
    public class NotificationManager : IHostedService, IMessageHandlerCallback
    {
        IMessageHandler _messageHandler;
        private readonly ILogger<NotificationManager> _logger;
        private readonly IMailService _mailService;

        public NotificationManager(IMessageHandler messageHandler,
            IMailService mailService,
            ILogger<NotificationManager> logger)
        {
            _logger = logger;
            _mailService = mailService;
            _messageHandler = messageHandler;
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
                await _mailService.SendEmailAsync(ur, subject, ur.ConfrimationLink, true, ur.FirstName);
            }
            catch (Exception e)
            {
                _logger.LogError("Sending email exception: "+e.Message);
            }
        }

    }
}
