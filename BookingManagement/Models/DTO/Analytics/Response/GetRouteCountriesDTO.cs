using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Route.Response
{
    public class GetRouteCountriesDTO
    {
        public string CountryName { get; set; }
        public decimal Income { get; set; }
    }
}
