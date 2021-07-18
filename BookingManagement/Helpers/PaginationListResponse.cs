using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Helpers
{
    public class PaginationListResponse<T>
    {
        public int PageOffset { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsCount { get; set; }
        public int ItemsPerPage { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
