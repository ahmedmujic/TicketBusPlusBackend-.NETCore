using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Route
{
    public class AddRouteRequestDTO
    {
        public int StartStationId { get; set; }
        public int EndStationId { get; set; }
        public string CompanyId { get; set; }
        public int BusId { get; set; }
        public decimal Price { get; set; }
        public List<DateTime> Dates { get; set; }
    }
}
