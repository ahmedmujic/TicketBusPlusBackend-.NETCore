using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Route.Response
{
    public class RouteResponseDTO
    {
        public string BusName { get; set; }
        public  decimal Price { get; set; }
        public int Sells { get; set; }
        public string StartingStation { get; set; }
        public string EndingStation { get; set; }
        public IEnumerable<DateTime> Dates{ get; set; }
    }
}
