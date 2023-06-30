using Microsoft.EntityFrameworkCore;

namespace PitchLogAPI.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int ResourceCount { get; private set; }

        public int PageNum { get; private set; }

        public int PageSize { get; private set; }

        public int PageCount { get; private set; }

        public PagedList(IEnumerable<T> items, int resourceCount, int pageNum, int pageSize)
        {
            AddRange(items);

            ResourceCount = resourceCount;
            PageNum = pageNum;
            PageSize = pageSize;
            PageCount = (int) Math.Ceiling(ResourceCount / (double)pageSize);
        }

        public static async Task<PagedList<T>> Create(IQueryable<T> source, int pageNum, int pageSize)
        {
            int count = source.Count();
            var items = await source.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNum, pageSize);
        }
    }
}
