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
        public List<int> Seats { get; set; }
        public List<string> SeatNumbers { get; set; }
        public decimal Amount { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonIgnore]
        public string Email { get; set; }
    }
}
