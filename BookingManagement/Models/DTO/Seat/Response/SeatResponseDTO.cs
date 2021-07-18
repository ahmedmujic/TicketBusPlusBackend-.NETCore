using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Seat.Response
{
    public class SeatResponseDTO
    {
        public int Id { get; set; }
        public string SeatCode { get; set; }
        public bool Checked { get; set; }
        public int BusId { get; set; }
    }
}
