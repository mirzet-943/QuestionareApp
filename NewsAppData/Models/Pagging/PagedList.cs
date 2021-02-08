using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsAPI.Models.Pagging
{
    public class PagedList<T> 
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public List<T> Items { get; set; }



        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = new List<T>(items);
        }
        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source;
            if (pageNumber == 0)
                return new PagedList<T>(items.ToList(), count, 0, 0);
            if (count > (pageNumber - 1) * pageSize)
                items = items.Skip((pageNumber - 1) * pageSize);
            if (items.Count() > pageSize)
                items = items.Take(pageSize);
            return new PagedList<T>(items.ToList(), count, pageNumber, pageSize);
        }
    }
}
