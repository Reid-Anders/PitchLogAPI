using PitchLogLib;
using PitchLogLib.Entities;

namespace PitchLogAPI.Services
{
    public interface IPitchLogRepository
    {
        Task<IEnumerable<Grade>> GetGrades();

        Task<IEnumerable<Grade>> GetGrades(GradeType gradeType);
    }
}
