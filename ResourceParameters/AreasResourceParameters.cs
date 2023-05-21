namespace PitchLogAPI.ResourceParameters
{
    public class AreasResourceParameters : PaginationResourceParameters
    {
        public string? Municipality
        {
            get => _municipality?.Trim();
            set => _municipality = value;
        }
        private string? _municipality = null;

        public string? Region
        {
            get => _region?.Trim();
            set => _region = value;
        }
        private string? _region = null;

        public string? Country
        {
            get => _country?.Trim();
            set => _country = value;
        }
        private string? _country = null;

        public string? SearchQuery { get; set; }

        public string? OrderBy { get; set; }
    }
}
