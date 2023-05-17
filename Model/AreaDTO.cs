using System.ComponentModel.DataAnnotations;

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
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Municipality { get; set; } = string.Empty;

        [Required]
        [MaxLength(2)]
        public string Region { get; set; } = string.Empty;

        [Required]
        [MaxLength(2)]
        public string Country { get; set; } = string.Empty;
    }

    public class AreaForUpdateDTO
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        public string Municipality { get; set; } = string.Empty;

        [Required]
        [MaxLength(2)]
        public string Region { get; set; } = string.Empty;

        [Required]
        [MaxLength(2)]
        public string Country { get; set; } = string.Empty;
    }
}
