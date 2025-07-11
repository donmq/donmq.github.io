using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API.Helpers.Params
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

         public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, 
            int pageNumber, int pageSize, bool isPaging = true) {
            var count = await source.CountAsync();
            if (isPaging)
            {
                var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                return new PagedList<T>(items, count, pageNumber, pageSize);
            }
            else 
            {
                var items = await source.ToListAsync();
                return new PagedList<T>(items, count, pageNumber, pageSize);
            }
        }
        public static PagedList<T> Create(List<T> source,
            int pageNumber, int pageSize, bool isPaging = true) {
            if (isPaging)
            {
                var count = source.Count();
                var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                return new PagedList<T>(items, count, pageNumber, pageSize);
            }
            else
            {
                var count = source.Count();
                var items = source.ToList();
                return new PagedList<T>(items, count, pageNumber, pageSize);
            }

        }
    }
}