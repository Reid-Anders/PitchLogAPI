using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public AreaController(IAreasRepository repository, IMapper mapper, ProblemDetailsFactory problemDetailsFactory)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));
        }

        [HttpGet(Name = nameof(GetAreas))]
        public async Task<IActionResult> GetAreas([FromQuery] AreasResourceParameters parameters)
        {
            var areas = await _repository.GetCollection(parameters);
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
            var area = await _repository.GetByID(ID);

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
            if(await _repository.Exists(areaForCreation.Name))
            {
                return BadRequest(_problemDetailsFactory.CreateProblemDetails(
                    HttpContext, statusCode: 400, detail: $"Area with the name {areaForCreation.Name} already exists."));
            }

            var areaToCreate = _mapper.Map<Area>(areaForCreation);
           
            _repository.Create(areaToCreate);
            await _repository.Save();

            var areaToReturn = _mapper.Map<AreaDTO>(areaToCreate);
            AddLinksToArea(areaToReturn);

            return CreatedAtRoute(nameof(GetAreaByID), new { areaToCreate.ID }, areaToReturn);
        }

        [HttpPut("{ID}", Name = nameof(UpdateAreaFull))]
        public async Task<IActionResult> UpdateAreaFull(int ID, AreaForUpdateDTO areaForUpdate)
        {
            var area = await _repository.GetByID(ID);

            if(area == null)
            {
                area = _mapper.Map<Area>(areaForUpdate);
                area.ID = ID;

                _repository.Create(area);
                await _repository.Save();

                var areaToReturn = _mapper.Map<AreaDTO>(area);

                return CreatedAtRoute(nameof(GetAreaByID), new { ID }, areaToReturn);
            }

            _mapper.Map(areaForUpdate, area);
            await _repository.Save();

            return NoContent();
        }

        [HttpPatch("{ID}", Name = nameof(UpdateAreaPartial))]
        public async Task<IActionResult> UpdateAreaPartial(AreaForUpdateDTO areaForUpdate)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{ID}", Name = nameof(DeleteArea))]
        public async Task<IActionResult> DeleteArea(int ID)
        {
            var areaToDelete = await _repository.GetByID(ID);

            if(areaToDelete == null)
            {
                return NotFound();
            }

            _repository.Delete(areaToDelete);
            _repository.Save();

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
