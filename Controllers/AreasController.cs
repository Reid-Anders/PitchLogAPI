using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
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
        private readonly IAreasRepository _areasRepository;
        private readonly IMapper _mapper;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public AreaController(IAreasRepository areasRepository, IMapper mapper, ProblemDetailsFactory problemDetailsFactory)
        {
            _areasRepository = areasRepository ?? throw new ArgumentNullException(nameof(areasRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
        }

        [HttpGet(Name = nameof(GetAreas))]
        public async Task<IActionResult> GetAreas([FromQuery] AreasResourceParameters parameters)
        {
            var areas = await _areasRepository.GetCollection(parameters);
            Response.AddPaginationHeaders(areas, Request.GetAbsoluteUri());

            var areasToReturn = _mapper.Map<IEnumerable<AreaDTO>>(areas);
            AddLinksToAreas(areasToReturn);

            var links = new List<LinkDTO>();
            links.Add(new LinkDTO(Url.Link(nameof(GetAreas), parameters), "self", "GET"));
            
            return Ok(new
            {
                resource = areasToReturn,
                links
            });
        }

        [HttpGet("{ID}", Name = nameof(GetAreaByID))]
        public async Task<IActionResult> GetAreaByID(int ID)
        {
            var area = await _areasRepository.GetByID(ID);

            if(area == null)
            {
                return NotFound();
            }

            var areaToReturn = _mapper.Map<AreaDTO>(area);
            AddLinksToArea(areaToReturn);

            return Ok(areaToReturn);
        }

        [HttpPost(Name = nameof(CreateArea))]
        public async Task<IActionResult> CreateArea(AreaForCreationDTO areaForCreation)
        {
            if(await _areasRepository.Exists(areaForCreation.Name))
            {
                return BadRequest(_problemDetailsFactory.CreateProblemDetails(
                    HttpContext, statusCode: 400, detail: $"Area with the name {areaForCreation.Name} already exists."));
            }

            var areaToCreate = _mapper.Map<Area>(areaForCreation);
           
            _areasRepository.Create(areaToCreate);
            await _areasRepository.Save();

            var areaToReturn = _mapper.Map<AreaDTO>(areaToCreate);
            AddLinksToArea(areaToReturn);

            return CreatedAtRoute(nameof(GetAreaByID), new { areaToCreate.ID }, areaToReturn);
        }

        [HttpPut("{ID}", Name = nameof(UpdateAreaFull))]
        public async Task<IActionResult> UpdateAreaFull(int ID, AreaForUpdateDTO areaForUpdate)
        {
            var area = await _areasRepository.GetByID(ID);

            if(area == null)
            {
                return NotFound();
            }

            _mapper.Map(areaForUpdate, area);
            await _areasRepository.Save();

            return NoContent();
        }

        [HttpPatch("{ID}", Name = nameof(UpdateAreaPartial))]
        public async Task<IActionResult> UpdateAreaPartial(int ID, JsonPatchDocument<AreaForUpdateDTO> patchDoc)
        {
            var area = await _areasRepository.GetByID(ID);

            if(area == null)
            {
                return NotFound();
            }

            var areaToPatch = _mapper.Map<AreaForUpdateDTO>(area);
            patchDoc.ApplyTo(areaToPatch);

            if(!TryValidateModel(areaToPatch))
            {
                return (IActionResult)HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>()
                    .Value.InvalidModelStateResponseFactory(ControllerContext);
            }

            _mapper.Map(areaToPatch, area);

            await _areasRepository.Save();

            return NoContent();
        }

        [HttpDelete("{ID}", Name = nameof(DeleteArea))]
        public async Task<IActionResult> DeleteArea(int ID)
        {
            var areaToDelete = await _areasRepository.GetByID(ID);

            if(areaToDelete == null)
            {
                return NotFound();
            }

            _areasRepository.Delete(areaToDelete);
            _areasRepository.Save();

            return NoContent();
        }

        private void AddLinksToArea(AreaDTO area)
        {
            var id = new { area.ID };
            area.Links.Add(new LinkDTO(Url.Link(nameof(GetAreaByID), id), "self", "GET"));
            area.Links.Add(new LinkDTO(Url.Link(nameof(CreateArea), new { }), "create", "POST"));
            area.Links.Add(new LinkDTO(Url.Link(nameof(UpdateAreaFull), id), "update", "PUT"));
            area.Links.Add(new LinkDTO(Url.Link(nameof(UpdateAreaPartial), id), "update_partial", "PATCH"));
            area.Links.Add(new LinkDTO(Url.Link(nameof(DeleteArea), id), "delete", "DELETE"));
        }

        private void AddLinksToAreas(IEnumerable<AreaDTO> areas)
        {
            foreach(var area in areas)
            {
                AddLinksToArea(area);
            }
        }
    }
}
