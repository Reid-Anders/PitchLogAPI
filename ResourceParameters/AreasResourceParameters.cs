namespace PitchLogAPI.ResourceParameters
{
    public class AreasResourceParameters : BaseResourceParameters
    {
        public string? Municipality 
        { 
            get => _municipality?.Trim().ToLower(); 
            set => _municipality = value; 
        }
        private string? _municipality = null;

        public string? Region
        {
            get => _region?.Trim().ToLower();
            set => _region = value;
        }
        private string? _region = null;

        public string? Country
        {
            get => _country?.Trim().ToLower();
            set => _country = value;
        }
        private string? _country = null;
    }
}
