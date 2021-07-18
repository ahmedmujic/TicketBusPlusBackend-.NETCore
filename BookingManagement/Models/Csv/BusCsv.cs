using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingManagement.Models.Csv
{
    public class BusCsv
    {
        public string Name { get; set; }
        public int NumberOfSeats { get; set; }
        [JsonIgnore]
        public string CompanyId { get; set; }
    }
}
