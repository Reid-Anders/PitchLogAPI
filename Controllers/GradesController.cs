using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;
using PitchLogAPI.Services;
using PitchLogData;
using PitchLogLib;

namespace PitchLogAPI.Controllers
{
    [Route("api/grades")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly IGradesRepository _repository;
        private readonly IMapper _mapper;

        public GradesController(IGradesRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Private, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = true)]
        public async Task<IActionResult> GetAllGrades([FromQuery] GradesResourceParameters parameters)
        {
            return await GetGrades(parameters);
        }

        [HttpGet("YDS")]
        public async Task<IActionResult> GetYDSGrades([FromQuery] GradesResourceParameters parameters)
        {
            return await GetGrades(parameters, GradeType.YDS);
        }

        [HttpGet("vGrades")]
        public async Task<IActionResult> GetVGrades([FromQuery] GradesResourceParameters parameters)
        {
            return await GetGrades(parameters, GradeType.VGrade);
        }

        [HttpGet("french")]
        public async Task<IActionResult> GetFrenchGrades([FromQuery] GradesResourceParameters parameters)
        {
            return await GetGrades(parameters, GradeType.French);
        }

        [HttpGet("britishTrad")]
        public async Task<IActionResult> GetBritishTradGrades([FromQuery] GradesResourceParameters parameters)
        {
            return await GetGrades(parameters, GradeType.BritishTrad);
        }

        [HttpGet("font")]
        public async Task<IActionResult> GetFontGrades([FromQuery] GradesResourceParameters parameters)
        {
            return await GetGrades(parameters, GradeType.Font);
        }

        [HttpGet("australian")]
        public async Task<IActionResult> GetAustralianGrades([FromQuery] GradesResourceParameters parameters)
        {
            return await GetGrades(parameters, GradeType.Australian);
        }

        [HttpGet("southAfrican")]
        public async Task<IActionResult> GetSouthAfricanGrades([FromQuery] GradesResourceParameters parameters)
        {
            return await GetGrades(parameters, GradeType.SouthAfrican);
        }

        private async Task<IActionResult> GetGrades(GradesResourceParameters parameters, GradeType type = GradeType.All)
        {
            var grades = await _repository.GetCollection(parameters, type);

            if (grades.Count() == 0)
            {
                return NoContent();
            }

            Response.AddPaginationHeaders(grades, Request.GetAbsoluteUri());

            return Ok(_mapper.Map<IEnumerable<GradeDTO>>(grades));
        }
    }
}
