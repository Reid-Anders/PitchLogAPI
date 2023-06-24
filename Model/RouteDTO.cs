namespace PitchLogAPI.Model
{
    public class RouteDTO : ClimbDTO
    {
        public int? Length { get; set; }
    }

    public class RouteForCreationDTO
    {
        public string Name { get; set; }

        public int GradeID { get; set; }

        public int? Length { get; set; }
    }

    public class RouteForUpdateDTO
    {
        public string Name { get; set; }

        public int GradeID { get; set; }

        public int? Length { get; set; }

        public int SectorID { get; set; }
    }
}
