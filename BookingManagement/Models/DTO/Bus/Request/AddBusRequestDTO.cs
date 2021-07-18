using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Bus.Request
{
    public class AddBusRequestDTO
    {
        public string Name { get; set; }
        public int NumberOfSeats { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
    }
}
