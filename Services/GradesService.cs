using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;
using PitchLogData;
using PitchLogLib;
using PitchLogLib.Entities;

namespace PitchLogAPI.Services
{
    public class GradesService : BaseService, IGradesService
    {
        public GradesService(PitchLogContext context,
            IMapper mapper,
            IHttpContextAccessor accessor,
            ProblemDetailsFactory problemDetailsFactory) : base(context, mapper, accessor, problemDetailsFactory)
        {

        }

        public async Task<GradeDTO> GetGrade(int gradeID)
        {
            var grade = await _context.Grades.FindAsync(gradeID);

            if(grade == null)
            {
                throw new RestException(_problemDetailsFactory.CreateProblemDetails(_contextAccessor.HttpContext,
                    statusCode: 404,
                    detail: $"Grade with id {gradeID} not found. Please ensure you have the correct ID"));
            }

            return _mapper.Map<GradeDTO>(grade);
        }

        public async Task<PagedList<GradeDTO>> GetAllGrades(GradesResourceParameters parameters)
        {
            return await GetGrades(parameters, GradeType.All);
        }

        public async Task<PagedList<GradeDTO>> GetAustralianGrades(GradesResourceParameters parameters)
        {
            return await GetGrades(parameters, GradeType.Australian);
        }

        public async Task<PagedList<GradeDTO>> GetFontGrades(GradesResourceParameters parameters)
        {
            return await GetGrades(parameters, GradeType.Font);
        }

        public async Task<PagedList<GradeDTO>> GetFrenchGrades(GradesResourceParameters parameters)
        {
            return await GetGrades(parameters, GradeType.French);
        }

        public async Task<PagedList<GradeDTO>> GetSouthAfricanGrades(GradesResourceParameters parameters)
        {
            return await GetGrades(parameters, GradeType.SouthAfrican);
        }

        public async Task<PagedList<GradeDTO>> GetVGrades(GradesResourceParameters parameters)
        {
            return await GetGrades(parameters, GradeType.VGrade);
        }

        public async Task<PagedList<GradeDTO>> GetYDSGrades(GradesResourceParameters parameters)
        {
            return await GetGrades(parameters, GradeType.YDS);
        }

        private async Task<PagedList<GradeDTO>> GetGrades(GradesResourceParameters parameters, GradeType type)
        {
            PagedList<Grade> grades;

            if (type == GradeType.All)
            {
                grades = await PagedList<Grade>.Create(_context.Grades, parameters.PageNum, parameters.PageSize);
            }
            else
            {
                grades = await PagedList<Grade>.Create(
                    _context.Grades.Where(grade => grade.Type == type),
                    parameters.PageNum,
                    parameters.PageSize);
            }

            return _mapper.Map<PagedList<GradeDTO>>(grades);
        }
    }
}
