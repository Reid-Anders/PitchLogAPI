using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;

namespace PitchLogAPI.Services
{
    public interface IGradesService
    {
        public Task<GradeDTO> GetGrade(int gradeID);

        public Task<PagedList<GradeDTO>> GetAllGrades(GradesResourceParameters parameters);

        public Task<PagedList<GradeDTO>> GetYDSGrades(GradesResourceParameters parameters);

        public Task<PagedList<GradeDTO>> GetVGrades(GradesResourceParameters parameters);

        public Task<PagedList<GradeDTO>> GetFontGrades(GradesResourceParameters parameters);

        public Task<PagedList<GradeDTO>> GetFrenchGrades(GradesResourceParameters parameters);

        public Task<PagedList<GradeDTO>> GetAustralianGrades(GradesResourceParameters parameters);

        public Task<PagedList<GradeDTO>> GetSouthAfricanGrades(GradesResourceParameters parameters);
    }
}
