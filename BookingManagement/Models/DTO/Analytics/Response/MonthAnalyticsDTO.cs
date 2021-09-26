using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Analytics.Response
{
    public class MonthAnalyticsDTO
    {
        public int Month { get; set; }
        public decimal Income { get; set; }
        public decimal Max { get; set; }
    }
}
