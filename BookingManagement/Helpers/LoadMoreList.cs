using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Helpers
{
    public class LoadMoreList<T> : List<T>
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int TotalItems { get; set; }
        public bool HasNext => ((Offset * Limit) + Limit) < TotalItems;

        public LoadMoreList(List<T> items, int offset, int limit, int totalItems)
        {
            Offset = offset;
            Limit = limit;
            TotalItems = totalItems;

            AddRange(items);
        }

        public async static Task<LoadMoreList<T>> ToLoadMoreListAsync(IQueryable<T> source, int currentIndex, int numberOfElements)
        {
            var itemsCount = await source.CountAsync();
            var items = await source.Skip(currentIndex * numberOfElements).Take(numberOfElements).ToListAsync().ConfigureAwait(false);
            return new LoadMoreList<T>(items, currentIndex, numberOfElements, itemsCount);
        }
    }
}
