using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Messaging;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using NotificationService.Events;
using NotificationService.Interfaces;
using Serilog;

namespace NotificationService
{
    public class NotificationManager : IHostedService, IMessageHandlerCallback
    {
        IMessageHandler _messageHandler;
        private readonly IMailService _mailService;

        public NotificationManager(IMessageHandler messageHandler, IMailService mailService)
        {
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
                    case "UserRegistered":
                        await HandleAsync(messageObject.ToObject<UserRegistered>());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error while handling {messageType} event.");
            }

            return true;
        }

        private async Task HandleAsync(UserRegistered ur)
        {
            try
            {

                await _mailService.SendEmailAsync(ur.Email, "Email Activation", ur.ConfrimationLink, true, ur.UserName);
            }
            catch (Exception e)
            {
                Log.Error("Sending email exception: "+e.Message);
            }
        }
       
    }
}
