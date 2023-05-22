namespace PitchLogAPI.ResourceParameters
{
    public class SectorsResourceParameters : BaseResourceParameters
    {
        public string? Name
        {
            get => _name?.Trim();
            set => _name = value;
        }
        public string? _name = null;

        public int? Approach { get; set; }

        public string? Aspect
        {
            get => _aspect?.Trim();
            set => _aspect = value;
        }
        private string? _aspect = null;
    }
}
