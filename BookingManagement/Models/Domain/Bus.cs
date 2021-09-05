using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.Domain
{
    public class Bus
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfSeats { get; set; }
        public string BusPicture { get; set; }
        public string CompanyId { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }
}
