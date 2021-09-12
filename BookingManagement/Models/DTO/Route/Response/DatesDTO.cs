using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Route.Response
{
    public class DatesDTO
    {
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public double Duration { get; set; }
    }
}
