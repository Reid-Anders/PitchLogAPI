using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogLib;
using PitchLogLib.Entities;

namespace PitchLogAPI.Services
{
    public interface IPitchLogRepository
    {
        Task<PagedList<Grade>> GetGrades(GradesResourceParameters parameters, GradeType gradeType = GradeType.All);

        Task<PagedList<Area>> GetAreas(AreasResourceParameters parameters);

        Task<Area> GetArea(int ID);

        void AddArea(Area area);

        Task<bool> SaveAsync();
    }
}
