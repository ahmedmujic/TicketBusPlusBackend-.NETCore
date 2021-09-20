using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Route.Response
{
    public class TimeStatDataResponseDTO
    {
        public decimal Income { get; set; }
        public decimal LastMonthIncome { get; set; }
        public decimal LastYearIncome { get; set; }

    }
}
