using Microsoft.EntityFrameworkCore;
using PitchLogAPI.ResourceParameters;
using PitchLogData;
using PitchLogLib;
using PitchLogLib.Entities;
using System.Linq;

namespace PitchLogAPI.Services
{
    public class PitchLogRepository : IPitchLogRepository
    {
        private readonly PitchLogContext _context;

        public PitchLogRepository(PitchLogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<PagedList<Grade>> GetGrades(GradesResourceParameters parameters, GradeType type = GradeType.All)
        {
            if(type == GradeType.All)
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
