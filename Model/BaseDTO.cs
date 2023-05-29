using System.Text.Json.Serialization;

namespace PitchLogAPI.Model
{
    public class BaseDTO
    {
        public IList<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    }
}
