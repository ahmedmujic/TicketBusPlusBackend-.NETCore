using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Bus.Request
{
    public class PaginationRequestDTO
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
    }
}
