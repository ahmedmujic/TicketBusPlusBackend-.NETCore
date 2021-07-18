using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.Domain
{
    public class Seat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SeatCode { get; set; }
        public bool Checked { get; set; }
        public Bus Bus { get; set; }
        public int BusId { get; set; }

        public Tickets Ticket { get; set; }
    }
}
