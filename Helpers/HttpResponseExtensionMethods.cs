using PitchLogAPI.Services;

namespace PitchLogAPI.Helpers
{
    public static class HttpResponseExtensionMethods
    {
        public static void AddPaginationHeaders<T>(this HttpResponse Response, PagedList<T> data)
        {
            AddPaginationHeaders(Response, data, string.Empty);
        }   

        public static void AddPaginationHeaders<T>(this HttpResponse Response, PagedList<T> data, string uri)
        {
            var paginationData = new PaginationMetaData(data.ResourceCount, data.PageNum, data.PageSize, data.PageCount, uri);

            if (!string.IsNullOrEmpty(paginationData.ResourceCount))
            {
                Response.Headers.Add("x-resource-count", paginationData.ResourceCount);
            }

            if (!string.IsNullOrEmpty(paginationData.PageNum))
            {
                Response.Headers.Add("x-pagination-pageNum", paginationData.PageNum);
            }

            if (!string.IsNullOrEmpty(paginationData.PageSize))
            {
                Response.Headers.Add("x-pagination-pageSize", paginationData.PageSize);
            }

            if (!string.IsNullOrEmpty(paginationData.PageCount))
            {
                Response.Headers.Add("x-pagination-pagecount", paginationData.PageCount);
            }

            if (!string.IsNullOrEmpty(paginationData.NextPage))
            {
                Response.Headers.Add("x-pagination-nextPage", paginationData.NextPage);
            }

            if (!string.IsNullOrEmpty(paginationData.PrevPage))
            {
                Response.Headers.Add("x-pagination-prevPage", paginationData.PrevPage);
            }

            if (!string.IsNullOrEmpty(paginationData.LastPage))
            {
                Response.Headers.Add("x-pagination-lastPage", paginationData.LastPage);
            }

            if (!string.IsNullOrEmpty(paginationData.FirstPage))
            {
                Response.Headers.Add("x-pagination-firstPage", paginationData.FirstPage);
            }
        }
    }
}
