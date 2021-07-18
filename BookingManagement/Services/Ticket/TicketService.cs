using AuthService.Events;
using BookingManagement.Models.Domain;
using BookingManagement.Models.DTO.Ticket.Request;
using BookingManagement.Services.Ticket.Interface;
using Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotificationService.Constants;
using NotificationService.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Services.Ticket
{
    public class TicketService : ITicketService
    {
        private readonly BookingManagementDbContext _dbContext;
        private readonly ILogger<TicketService> _logger;
        private readonly IMessagePublisher _messagePublisher;

        public TicketService(BookingManagementDbContext dbContext,
            ILogger<TicketService> logger,
            IMessagePublisher messagePublisher)
        {
            _dbContext = dbContext;
            _logger = logger;
            _messagePublisher = messagePublisher;
        }

        public async Task<bool> BookTicketAsync(BookTicketDTO request)
        {
            try
            {
                var route = _dbContext.Routes.FirstOrDefault(r => r.Id == request.RouteId);
                List<Tickets> tickets = new List<Tickets>();
                foreach(var seat in request.Seats)
                {
                    Tickets ticket = new Tickets
                    {
                        Date = DateTime.Now,
                        IsCanceled = false,
                        RouteId = request.RouteId,
                        SeatId = seat,
                        UserId = request.UserId                        
                    };

                    route.SellCounter++;
                    tickets.Add(ticket);
                }
                var array = tickets.Select(x => x.SeatId).ToArray();

                _dbContext.Database.ExecuteSqlRaw($"UPDATE [Seats] SET [Checked] = 'True' WHERE [Id] IN ({string.Join(", ", array)})");
                await _dbContext.Tickets.AddRangeAsync(tickets);

                
               

                var isSucessful =  await _dbContext.SaveChangesAsync() >= 0;
                if (isSucessful)
                {

                    InvoiceSend e = new InvoiceSend(new Guid(), request.Amount, null, request.Email, request.SeatNumbers, null);
                    await _messagePublisher.PublishMessageAsync(MessageTypes.InvoiceSend, e, "");
                }

                return isSucessful;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(BookTicketAsync));
                throw;
            }
        }
    }
}
