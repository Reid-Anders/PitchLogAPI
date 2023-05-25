using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Attributes;

namespace PitchLogAPI.ResourceParameters
{
    public class SectorsResourceParameters : BaseResourceParameters
    {
        [ModelBinder(typeof(MultiFieldModelBinder))]
        public IEnumerable<string>? Approach { get; set; } = new List<string>();

        public string? Aspect
        {
            get => _aspect?.Trim();
            set => _aspect = value;
        }
        private string? _aspect = null;
    }
}
