namespace PitchLogAPI.ResourceParameters
{
    public class SectorsResourceParameters : BaseResourceParameters
    {
        public string? Approach
        {
            get => _approach?.Trim();
            set => _approach = value;
        }
        private string? _approach = null;

        public string? Aspect
        {
            get => _aspect?.Trim();
            set => _aspect = value;
        }
        private string? _aspect = null;
    }
}
