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
        private readonly IAreasRepository _repository;
        private readonly IMapper _mapper;

        public AreaController(IAreasRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        [HttpGet(Name = "GetAreas")]
        [Produces("application/json", "application/randerson.hateoas+json")]
        public async Task<IActionResult> GetAreas([FromQuery] AreasResourceParameters parameters)
        {
            var areas = await _repository.GetCollection(parameters);

            if(areas.Count() == 0)
            {
                return NoContent();
            }

            var areasToReturn = _mapper.Map<IEnumerable<AreaDTO>>(areas);
            Response.AddPaginationHeaders(areas, Request.GetAbsoluteUri());

            if (Request.Headers.Accept.Contains("application/randerson.hateoas+json"))
            {
                var links = new List<LinkDTO>();
                links.Add(new LinkDTO(Url.Link("GetAreas", parameters), "self", "GET"));

                return Ok(new
                {
                    resource = areasToReturn,
                    links = links
                });
            }

            return Ok(areasToReturn);
        }

        [HttpGet("{ID}", Name = "GetArea")]
        [Produces("application/json", "application/randerson.hateoas+json")]
        public async Task<IActionResult> GetAreaByID(int ID)
        {
            var area = await _repository.GetByID(ID);

            if(area == null)
            {
                return NotFound();
            }

            var areaToReturn = _mapper.Map<AreaDTO>(area);

            if (Request.IncludeHateoas())
            {
                var links = new List<LinkDTO>();
                links.Add(new LinkDTO(Url.Link("GetArea", new { ID }), "self", "GET"));
                links.Add(new LinkDTO(Url.Link("CreateArea", null), "create", "POST"));

                return Ok(new ResourceAndLinksDTO(areaToReturn, links));
            }

            return Ok(areaToReturn);
        }

        [HttpPost(Name = "CreateArea")]
        [Produces("application/json", "application/randerson.hateoas+json")]
        public async Task<IActionResult> CreateArea(AreaForCreationDTO areaForCreation)
        {
            var areaToCreate = _mapper.Map<Area>(areaForCreation);
           
            _repository.Create(areaToCreate);
            await _repository.Save();

            var areaToReturn = _mapper.Map<AreaDTO>(areaToCreate);

            if(Request.IncludeHateoas())
            {
                var links = new List<LinkDTO>();
                links.Add(new LinkDTO(Url.Link("GetArea", new { areaToReturn.ID }), "self", "GET"));

                return Ok(new ResourceAndLinksDTO(areaToReturn, links));
            }

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
