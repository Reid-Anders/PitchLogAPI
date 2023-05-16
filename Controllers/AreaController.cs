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
            var areaToCreate = _mapper.Map<Area>(areaForCreation);
           
            _repository.Create(areaToCreate);
            await _repository.Save();

            var areaToReturn = _mapper.Map<AreaDTO>(areaToCreate);
            AddLinksToArea(areaToReturn);

            return CreatedAtRoute(nameof(GetAreaByID), new { areaToCreate.ID }, areaToReturn);
        }

        [HttpPut("{ID}")]
        public async Task<IActionResult> UpdateAreaFull(AreaForUpdateDTO areaForUpdate)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{ID}")]
        public async Task<IActionResult> UpdateAreaPartial(AreaForUpdateDTO areaForUpdate)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteArea(int ID)
        {
            throw new NotImplementedException();
        }

        private void AddLinksToArea(AreaDTO area)
        {
            area.Links.Add(new LinkDTO(Url.Link(nameof(GetAreaByID), area.ID), "self", "GET"));
            area.Links.Add(new LinkDTO(Url.Link(nameof(CreateArea), new { }), "create", "POST"));
            area.Links.Add(new LinkDTO(Url.Link(nameof(UpdateAreaFull), area.ID), "update", "PUT"));
            area.Links.Add(new LinkDTO(Url.Link(nameof(UpdateAreaPartial), area.ID), "update_partial", "PATCH"));
            area.Links.Add(new LinkDTO(Url.Link(nameof(DeleteArea), area.ID), "delete", "DELETE"));
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
