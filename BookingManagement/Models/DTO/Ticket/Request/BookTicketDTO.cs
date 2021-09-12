using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Ticket.Request
{
    public class BookTicketDTO
    {
        public  string RouteId { get; set; }
        public int SeatId { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
    }
}
