using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Town.Response
{
    public class TownResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
    }
}
