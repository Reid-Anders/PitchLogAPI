namespace PitchLogAPI.Model
{
    public class SectorDTO : LinkedDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int? Approach { get; set; }

        public string Aspect { get; set; } = string.Empty;

        //public IList<ClimbDTO> Climbs { get; set; } = new List<ClimbDTO>();
    }

    public class SectorForCreationDTO
    {

    }

    public class SectorForUpdateDTO
    {

    }
}
