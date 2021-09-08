using BookingManagement.Models.DTO.Bus.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Route.Request
{
    public class RoutesRequest : PaginationRequestDTO
    {
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public int? FromTownId { get; set; }
        public int? EndTownId { get; set; }
    }
}
