using AutoMapper;
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
    public class GradeController : ControllerBase
    {
        private readonly IPitchLogRepository _pitchLogRepository;
        private readonly IPaginationService _paginationService;
        private readonly IMapper _mapper;

        public GradeController(IPitchLogRepository pitchLogRepository, IMapper mapper, IPaginationService paginationService)
        {
            _pitchLogRepository = pitchLogRepository ?? throw new ArgumentNullException(nameof(pitchLogRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _paginationService = paginationService ?? throw new ArgumentNullException(nameof(paginationService));
        }

        [HttpGet]
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
            var grades = await _pitchLogRepository.GetGrades(parameters, type);

            if (grades.Count() == 0)
            {
                return NoContent();
            }

            _paginationService.AddPaginationHeaders(Response, 
                new PaginationMetaData(grades.ResourceCount, grades.PageNum, grades.PageSize, grades.PageCount, Request.GetAbsoluteUri()));

            return Ok(_mapper.Map<IEnumerable<GradeDTO>>(grades));
        }
    }
}
