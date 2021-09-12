using BookingManagement.Models.Domain;
using BookingManagement.Models.DTO.Ticket.Request;
using BookingManagement.Services.Ticket.Interface;
using Microsoft.Extensions.Logging;
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

        public TicketService(BookingManagementDbContext dbContext,
            ILogger<TicketService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> BookTicketAsync(BookTicketDTO request)
        {
            try
            {
                Tickets ticket = new Tickets
                {
                    Date = DateTime.Now,
                    IsCanceled = false,
                    RouteId = request.RouteId,
                    SeatId = request.SeatId,
                    UserId = request.UserId
                };

                await _dbContext.Tickets.AddAsync(ticket);
                return await _dbContext.SaveChangesAsync() >= 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(BookTicketAsync));
                throw;
            }
        }
    }
}
