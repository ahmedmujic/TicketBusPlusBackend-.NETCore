using BookingManagement.Models.DTO.Ticket.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Services.Ticket.Interface
{
    public interface ITicketService
    {
        public Task<bool> BookTicketAsync(BookTicketDTO request);
    }
}
