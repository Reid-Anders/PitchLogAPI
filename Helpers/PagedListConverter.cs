using AutoMapper;

namespace PitchLogAPI.Helpers
{
    public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
    {
        public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
        {
            var mappedList = context.Mapper.Map<IEnumerable<TDestination>>(source);

            return new PagedList<TDestination>(mappedList, source.ResourceCount, source.PageNum, source.PageSize);
        }
    }
}
