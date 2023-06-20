namespace PitchLogAPI.ResourceParameters
{
    public class RoutesResourceParameters : BaseResourceParameters
    {
        public IEnumerable<string>? Grade { get; set; } = new List<string>();
    }
}
