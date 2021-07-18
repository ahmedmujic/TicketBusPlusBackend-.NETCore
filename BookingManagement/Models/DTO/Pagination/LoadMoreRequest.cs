using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingManagement.Models.DTO.Bus.Request
{
    public class LoadMoreRequest
    {
        public int CurrentOffset { get; set; }
        public int ItemsPerOffset { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
    }
}
