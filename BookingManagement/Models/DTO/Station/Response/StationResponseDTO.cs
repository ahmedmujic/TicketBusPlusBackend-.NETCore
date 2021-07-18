using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Station.Response
{
    public class StationResponseDTO
    {
        public int StationId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
    }
}
