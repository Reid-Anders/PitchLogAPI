namespace PitchLogAPI.Services
{
    public class PaginationService : IPaginationService
    {
        public void AddPaginationHeaders(HttpResponse response, PaginationMetaData paginationData)
        {
            if(!string.IsNullOrEmpty(paginationData.ResourceCount))
            {
                response.Headers.Add("x-resource-count", paginationData.ResourceCount);
            }

            if(!string.IsNullOrEmpty(paginationData.PageNum))
            {
                response.Headers.Add("x-pagination-pageNum", paginationData.PageNum);
            }

            if(!string.IsNullOrEmpty(paginationData.PageSize))
            {
                response.Headers.Add("x-pagination-pageSize", paginationData.PageSize);
            }

            if(!string.IsNullOrEmpty(paginationData.PageCount))
            {
                response.Headers.Add("x-pagination-pagecount", paginationData.PageCount);
            }

            if(!string.IsNullOrEmpty(paginationData.NextPage))
            {
                response.Headers.Add("x-pagination-nextPage", paginationData.NextPage);
            }

            if(!string.IsNullOrEmpty(paginationData.PrevPage))
            {
                response.Headers.Add("x-pagination-prevPage", paginationData.PrevPage);
            }

            if(!string.IsNullOrEmpty(paginationData.LastPage))
            {
                response.Headers.Add("x-pagination-lastPage", paginationData.LastPage);
            }

            if(!string.IsNullOrEmpty(paginationData.FirstPage))
            {
                response.Headers.Add("x-pagination-firstPage", paginationData.FirstPage);
            }
        }
    }
}
