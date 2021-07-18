using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Route.Response
{
    public class RouteResponseDTO
    {
        public string Id { get; set; }
        public int BusId { get; set; }
        public string BusName { get; set; }
        public  decimal Price { get; set; }
        public int Sells { get; set; }
        public string StartingStation { get; set; }
        public string EndingStation { get; set; }
        public string StartingTownLat { get; set; }
        public string EndingTownLat { get; set; }
        public string StartingTownLong{ get; set; }
        public string EndingTownLong { get; set; }
        public string StartingTown { get; set; }
        public string EndingTown { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public double Duration { get; set; }
    }
}
