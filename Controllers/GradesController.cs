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
        private readonly IGradesService _gradesService;
        private readonly IMapper _mapper;

        public GradesController(IGradesService gradesService, IMapper mapper)
        {
            _gradesService = gradesService ?? throw new ArgumentNullException(nameof(gradesService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Private, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = true)]
        public async Task<IActionResult> GetAllGrades([FromQuery] GradesResourceParameters parameters)
        {
            return Ok(await _gradesService.GetAllGrades(parameters));
        }

        [HttpGet("YDS")]
        public async Task<IActionResult> GetYDSGrades([FromQuery] GradesResourceParameters parameters)
        {
            return Ok(await _gradesService.GetYDSGrades(parameters));
        }

        [HttpGet("vGrades")]
        public async Task<IActionResult> GetVGrades([FromQuery] GradesResourceParameters parameters)
        {
            return Ok(await _gradesService.GetVGrades(parameters));
        }

        [HttpGet("french")]
        public async Task<IActionResult> GetFrenchGrades([FromQuery] GradesResourceParameters parameters)
        {
            return Ok(await _gradesService.GetFrenchGrades(parameters));
        }

        [HttpGet("font")]
        public async Task<IActionResult> GetFontGrades([FromQuery] GradesResourceParameters parameters)
        {
            return Ok(await _gradesService.GetFontGrades(parameters));
        }

        [HttpGet("australian")]
        public async Task<IActionResult> GetAustralianGrades([FromQuery] GradesResourceParameters parameters)
        {
            return Ok(await _gradesService.GetAustralianGrades(parameters));
        }

        [HttpGet("southAfrican")]
        public async Task<IActionResult> GetSouthAfricanGrades([FromQuery] GradesResourceParameters parameters)
        {
            return Ok(await _gradesService.GetSouthAfricanGrades(parameters));
        }
    }
}
