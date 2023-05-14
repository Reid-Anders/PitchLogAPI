using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogData;
using PitchLogLib;
using PitchLogLib.Entities;

namespace PitchLogAPI.Services
{
    public class GradesRepository : IGradesRepository
    {
        private readonly PitchLogContext _context;

        public GradesRepository(PitchLogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Grade> GetByID(int ID)
        {
            return await _context.Grades.FindAsync(ID);
        }

        public async Task<PagedList<Grade>> GetCollection(PaginationResourceParameters parameters)
        {
            return await PagedList<Grade>.Create(
                    _context.Grades,
                    parameters.PageNum,
                    parameters.PageSize);
        }

        public async Task<PagedList<Grade>> GetCollection(PaginationResourceParameters parameters, GradeType type)
        {
            if (type == GradeType.All)
            {
                return await PagedList<Grade>.Create(_context.Grades, parameters.PageNum, parameters.PageSize);
            }
            else
            {
                return await PagedList<Grade>.Create(
                    _context.Grades.Where(grade => grade.Type == type),
                    parameters.PageNum,
                    parameters.PageSize);
            }
        }
    }
}
