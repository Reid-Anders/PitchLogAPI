namespace PitchLogAPI.Model
{
    public class AreaDTO : LinkedDTO
    {
        public int ID { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Municipality { get; set; } = string.Empty;

        public string Region { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;
    }

    public class AreaForCreationDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Municipality { get; set; } = string.Empty;

        public string Region { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;
    }

    public class AreaForUpdateDTO
    {

    }
}
