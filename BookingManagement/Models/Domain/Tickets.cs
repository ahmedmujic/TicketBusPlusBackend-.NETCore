using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.Domain
{
    public class Tickets
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsCanceled { get; set; } = false;
        public string RouteId { get; set; }
        public Routes Route { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public string UserId { get; set; }
    }
}
