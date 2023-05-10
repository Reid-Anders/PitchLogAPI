using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;
using PitchLogAPI.Services;
using PitchLogLib.Entities;

namespace PitchLogAPI.Controllers
{
    [Route("api/Areas")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IPitchLogRepository _pitchLogRepository;
        private readonly IMapper _mapper;

        public AreaController(IPitchLogRepository repository, IMapper mapper)
        {
            _pitchLogRepository = repository ?? throw new ArgumentException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }


        [HttpGet]
        public async Task<IActionResult> GetAreas([FromQuery] AreasResourceParameters parameters)
        {
            var areas = await _pitchLogRepository.GetAreas(parameters);

            if(areas.Count() == 0)
            {
                return NoContent();
            }

            Response.AddPaginationHeaders(areas, Request.GetAbsoluteUri());

            return Ok(_mapper.Map<IEnumerable<AreaDTO>>(areas));
        }

        [HttpGet("{ID}", Name = "GetArea")]
        public async Task<IActionResult> GetAreaByID(int ID)
        {
            var area = await _pitchLogRepository.GetArea(ID);

            if(area == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AreaDTO>(area));
        }

        [HttpPost]
        public async Task<IActionResult> CreateArea(AreaForCreationDTO areaForCreation)
        {
            var areaToCreate = _mapper.Map<Area>(areaForCreation);

            _pitchLogRepository.AddArea(areaToCreate);
            await _pitchLogRepository.SaveAsync();

            var areaToReturn = _mapper.Map<AreaDTO>(areaToCreate);

            return CreatedAtRoute("GetArea", new { areaToCreate.ID }, areaToReturn);
        }

        //[HttpPut]
        //public async Task<IActionResult> UpdateAreaFull(AreaForUpdateDTO areaForUpdate)
        //{

        //}

        //[HttpPatch("{ID}")]
        //public async Task<IActionResult> UpdateAreaPartial(AreaForUpdateDTO areaForUpdate)
        //{

        //}

        //[HttpDelete("{ID}")]
        //public async Task<IActionResult> DeleteArea(int ID)
        //{

        //}
    }
}
