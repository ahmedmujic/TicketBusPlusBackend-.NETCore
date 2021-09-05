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
        public bool IsCanceled { get; set; }
        public Routes Route { get; set; }
        public int RouteId { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public string UserId { get; set; }
    }
}
