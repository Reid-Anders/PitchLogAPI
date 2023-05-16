using System.Text.Json.Serialization;

namespace PitchLogAPI.Model
{
    public class LinkedDTO
    {
        public IList<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    }
}
