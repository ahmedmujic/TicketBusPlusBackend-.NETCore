using BookingManagement.Models.DTO.Info.Request;
using BookingManagement.Services.Info.Interface;
using Messaging;
using Microsoft.Extensions.Logging;
using NotificationService.Constants;
using NotificationService.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Services.Info
{
    public class InfoService : IInfoService
    {
        private readonly ILogger<InfoService> _logger;
        private readonly IMessagePublisher _messagePublisher;

        public InfoService(
            ILogger<InfoService> logger,
            IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _messagePublisher = messagePublisher;
        }

        public async Task<bool> SendInfoEmailAsync(SendInfoMailDTO request)
        {
            try
            {
                InfoSend e = new InfoSend(new Guid(), request.FullName, request.Email, request.Phone, request.Message);
                await _messagePublisher.PublishMessageAsync(MessageTypes.InfoSend, e, "");
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, nameof(SendInfoEmailAsync));
                throw;
            }
        }
    }
}
