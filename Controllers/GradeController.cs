using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PitchLogAPI.Model;
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
        private readonly IMapper _mapper;

        public GradeController(IPitchLogRepository pitchLogRepository, IMapper mapper)
        {
            _pitchLogRepository = pitchLogRepository ?? throw new ArgumentNullException(nameof(pitchLogRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllGrades()
        {
            var grades = await _pitchLogRepository.GetGrades();

            return Ok(_mapper.Map<IEnumerable<GradeDTO>>(grades));
        }

        [HttpGet("YDS")]
        public async Task<IActionResult> GetYDSGrades()
        {
            return await GetGrades(GradeType.YDS);
        }

        [HttpGet("vGrades")]
        public async Task<IActionResult> GetVGrades()
        {
            return await GetGrades(GradeType.VGrade);
        }

        [HttpGet("french")]
        public async Task<IActionResult> GetFrenchGrades()
        {
            return await GetGrades(GradeType.French);
        }

        [HttpGet("britishTrad")]
        public async Task<IActionResult> GetBritishTradGrades()
        {
            return await GetGrades(GradeType.BritishTrad);
        }

        [HttpGet("font")]
        public async Task<IActionResult> GetFontGrades()
        {
            return await GetGrades(GradeType.Font);
        }

        [HttpGet("australian")]
        public async Task<IActionResult> GetAustralianGrades()
        {
            return await GetGrades(GradeType.Australian);
        }

        [HttpGet("southAfrican")]
        public async Task<IActionResult> GetSouthAfricanGrades()
        {
            return await GetGrades(GradeType.SouthAfrican);
        }

        private async Task<IActionResult> GetGrades(GradeType type)
        {
            var grades = await _pitchLogRepository.GetGrades(type);

            return Ok(_mapper.Map<IEnumerable<GradeDTO>>(grades));
        }
    }
}
