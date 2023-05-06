using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Grade>> GetGrades()
        {
            return await _context.Grades.ToListAsync();
        }

        public async Task<IEnumerable<Grade>> GetGrades(GradeType type)
        {
            return await _context.Grades.Where(grade => grade.Type == type).ToListAsync();
        }
    }
}
