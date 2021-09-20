using BookingManagement.Models.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Route.Response
{
    public class TopSellingResponseDTO : ResponseWrapper
    {
        public string RouteName { get; set; }
        public string RouteId { get; set; }
        public decimal Percentage { get; set; }
        [JsonIgnore]
        public string CompanyId { get; set; }
    }
}
