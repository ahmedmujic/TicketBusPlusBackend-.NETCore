using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Route.Response
{
    public class TimeStatDataResponseDTO
    {
        public decimal LastWeekIncome { get; set; }
        public int SinceLastWeek { get; set; }
        public decimal LastMonthIncome { get; set; }
        public int SinceMonthtWeek { get; set; }
        public decimal LastYearIncome { get; set; }
        public int SinceYeartWeek { get; set; }

    }
}
