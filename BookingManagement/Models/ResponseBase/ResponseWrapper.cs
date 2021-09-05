using BookingManagement.Models.ResponseBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingManagement.Models.Errors
{
    public class ResponseWrapper
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<Error> Errors { get; set; }
    }
}
