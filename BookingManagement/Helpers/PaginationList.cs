using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Helpers
{
    public class PaginationList<T> : List<T>
    {
        public int PageOffset { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsCount { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalPages => ItemsCount / PageOffset;

        public PaginationList(List<T> items, int offset, int totalItems, int itemsPerPage)
        {
            PageOffset = offset;
            ItemsCount = totalItems;
            ItemsPerPage = itemsPerPage;

            AddRange(items);
        }

        public async static Task<PaginationList<T>> ToPaginationListAsync(IQueryable<T> source, int pageOffset, int numberOfElements)
        {
            var itemsCount = await source.CountAsync();
            var items = await source.Skip(pageOffset * numberOfElements).Take(numberOfElements).ToListAsync().ConfigureAwait(false);
            return new PaginationList<T>(items, pageOffset, itemsCount, numberOfElements);
        }
    }
}
