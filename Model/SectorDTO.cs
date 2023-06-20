using System.ComponentModel.DataAnnotations;

namespace PitchLogAPI.Model
{
    public class SectorDTO : BaseDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int? Approach { get; set; }

        public string Aspect { get; set; } = string.Empty;

        //public IList<ClimbDTO> Climbs { get; set; } = new List<ClimbDTO>();
    }

    public class SectorForCreationDTO
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Range(0,1024)]
        public int? Approach { get; set; }

        public string Aspect { get; set; }
    }

    public class SectorForUpdateDTO
    {
        [MaxLength(256)]
        public string Name { get; set; }

        [Range(0, 1024)]
        public int? Approach { get; set; }

        public string Aspect { get; set; }
    }
}
