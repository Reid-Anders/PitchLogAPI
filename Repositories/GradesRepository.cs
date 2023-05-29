using Microsoft.EntityFrameworkCore;
using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogData;
using PitchLogLib;
using PitchLogLib.Entities;

namespace PitchLogAPI.Repositories
{
    public class GradesRepository : BaseRepository<Grade>, IGradesRepository
    {
        public GradesRepository(PitchLogContext context) : base(context)
        {

        }

        public override async Task<bool> Exists(int ID)
        {
            return await _context.Grades.AnyAsync(grade => grade.ID == ID);
        }

        public override async Task<PagedList<Grade>> GetCollection(BaseResourceParameters parameters)
        {
            return await PagedList<Grade>.Create(
                    _context.Grades,
                    parameters.PageNum,
                    parameters.PageSize);
        }

        public async Task<PagedList<Grade>> GetCollection(BaseResourceParameters parameters, GradeType type)
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
