using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Town
{
    public class TownRequest
    {
        public int Offset { get; set; }
        public int Elements { get; set; }
    }
}
