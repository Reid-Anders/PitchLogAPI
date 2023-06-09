﻿namespace PitchLogAPI.Helpers
{
    public class PaginationMetaData
    {
        public string ResourceCount { get; } = string.Empty;

        public string PageNum { get; } = string.Empty;

        public string PageSize { get; } = string.Empty;

        public string PageCount { get; } = string.Empty;

        public string NextPage { get; } = string.Empty;

        public string PrevPage { get; } = string.Empty;

        public string LastPage { get; } = string.Empty;

        public string FirstPage { get; } = string.Empty;

        public PaginationMetaData(int resourceCount, int pageNum, int pageSize, int pageCount)
        {
            ResourceCount = resourceCount.ToString();
            PageNum = pageNum.ToString();
            PageSize = pageSize.ToString();
            PageCount = pageCount.ToString();
        }

        public PaginationMetaData(int resourcecount, int pageNum, int pageSize, int pageCount, string uri)
            : this(resourcecount, pageNum, pageSize, pageCount)
        {
            if (!string.IsNullOrEmpty(uri))
            {
                var splitUri = uri.Split('?');
                string baseUri = splitUri[0] + "?";
                string existingQueryString = splitUri[1];

                if (pageNum < pageCount)
                {
                    NextPage = baseUri + $"pageNum={pageNum + 1}&pageSize={pageSize}&{existingQueryString}";
                }

                if (pageNum > 0)
                {
                    PrevPage = baseUri + $"pageNum={pageNum - 1}&pageSize={pageSize}&{existingQueryString}";
                }

                if (pageNum != 0)
                {
                    FirstPage = baseUri + $"pageNum=0&pageSize={pageSize}&{existingQueryString}";
                }

                if (pageNum != pageCount)
                {
                    LastPage = baseUri + $"pageNum={pageCount}&pageSize={pageSize}&{existingQueryString}";
                }
            }
        }
    }
}
