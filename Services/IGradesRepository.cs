using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogLib;
using PitchLogLib.Entities;

namespace PitchLogAPI.Services
{
    public interface IGradesRepository : IReadRepository<Grade>
    {
        Task<PagedList<Grade>> GetCollection(BaseResourceParameters parameters, GradeType type);
    }
}
