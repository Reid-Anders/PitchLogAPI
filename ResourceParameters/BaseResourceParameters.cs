namespace PitchLogAPI.ResourceParameters
{
    public abstract class BaseResourceParameters
    {
        const int MAX_PAGE_SIZE = 100;

        public int PageNum { get; set; } = 1;

        public int PageSize 
        {
            get => _pageSize;
            set => _pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value; 
        }

        private int _pageSize = 10;

        public string? SearchQuery { get; set; }

        public string? OrderBy { get; set; }
    }
}
