using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;
using PitchLogAPI.Repositories;
using PitchLogLib.Entities;
using PitchLogAPI.Helpers;
using Microsoft.Extensions.Options;

namespace PitchLogAPI.Controllers
{
    [Route("api/areas/{areaID}/sectors")]
    [ApiController]
    public class SectorsController : BasePitchLogController
    {
        private readonly ISectorsRepository _sectorsRepository;
        private readonly IAreasRepository _areasRepository;
        private readonly IMapper _mapper;

        private int areaID { get; set; }

        public SectorsController(ISectorsRepository sectorsRepository,
            IAreasRepository areasRepository,
            IMapper mapper)
        {
            _sectorsRepository = sectorsRepository ?? throw new ArgumentNullException(nameof(sectorsRepository));
            _areasRepository = areasRepository ?? throw new ArgumentNullException(nameof(areasRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{ID}", Name = nameof(GetSectorByID))]
        public async Task<IActionResult> GetSectorByID(int areaID, int ID)
        {
            if(!await _areasRepository.Exists(areaID))
            {
                return NotFound();
            }

            this.areaID = areaID;

            var sector = await _sectorsRepository.GetByID(ID);

            if(sector == null)
            {
                return NotFound();
            }

            var sectorToReturn = _mapper.Map<SectorDTO>(sector);
            AddLinksToResource(sectorToReturn);

            return Ok(sectorToReturn);
        }

        [HttpGet(Name = nameof(GetSectors))]
        public async Task<IActionResult> GetSectors(int areaID, [FromQuery] SectorsResourceParameters parameters)
        {
            if(!await _areasRepository.Exists(areaID))
            {
                return NotFound();
            }

            this.areaID = areaID;

            var sectors = await _sectorsRepository.GetCollection(parameters);
            Response.AddPaginationHeaders(sectors);

            var sectorsToReturn = _mapper.Map<IEnumerable<SectorDTO>>(sectors);
            AddLinksToResources(sectorsToReturn);

            var links = new List<LinkDTO>();
            links.Add(new LinkDTO(Url.Link(nameof(GetSectors), parameters), "self", "GET"));

            return Ok(new
            {
                resource = sectorsToReturn,
                links
            });
        }

        [HttpPost(Name = nameof(CreateSector))]
        public async Task<IActionResult> CreateSector(int areaID, SectorForCreationDTO sectorForCreation)
        {
            if(!await _areasRepository.Exists(areaID))
            {
                return NotFound();
            }

            this.areaID = areaID;

            var sector = _mapper.Map<Sector>(sectorForCreation);
            sector.AreaID = areaID;

            _sectorsRepository.Create(sector);
            await _sectorsRepository.SaveChanges();

            var sectorToReturn = _mapper.Map<SectorDTO>(sector);

            return CreatedAtRoute(nameof(GetSectorByID), new { areaID, sectorToReturn.ID }, sectorToReturn);
        }

        [HttpPut("{ID}", Name = nameof(UpdateSectorFull))]
        public async Task<IActionResult> UpdateSectorFull(int areaID, int ID, SectorForUpdateDTO sectorForUpdate)
        {
            if(!await _areasRepository.Exists(areaID))
            {
                return NotFound();
            }

            var sector = await _sectorsRepository.GetByID(ID);

            if(sector == null)
            {
                return NotFound();
            }

            _mapper.Map(sectorForUpdate, sector);
            await _sectorsRepository.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{ID}", Name = nameof(UpdateSectorPartial))]
        public async Task<IActionResult> UpdateSectorPartial(int areaID, int ID, JsonPatchDocument<SectorForUpdateDTO> patchDocument)
        {
            if(!await _areasRepository.Exists(areaID))
            {
                return NotFound();
            }

            var sector = await _sectorsRepository.GetByID(ID);

            if(sector == null)
            {
                return NotFound();
            }

            var sectorToPatch = _mapper.Map<SectorForUpdateDTO>(sector);
            patchDocument.ApplyTo(sectorToPatch);

            if(!TryValidateModel(sectorToPatch))
            {
                return HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>()
                    .Value.InvalidModelStateResponseFactory(ControllerContext);
            }

            _mapper.Map(sectorToPatch, sector);
            await _sectorsRepository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{ID}", Name = nameof(DeleteSector))]
        public async Task<IActionResult> DeleteSector(int areaID, int ID)
        {
            if(!await _areasRepository.Exists(areaID))
            {
                return NotFound();
            }

            var sector = await _sectorsRepository.GetByID(ID);

            if(sector == null)
            {
                return NotFound();
            }

            _sectorsRepository.Delete(sector);
            await _sectorsRepository.SaveChanges();

            return NotFound();
        }

        protected override void AddLinksToResource(BaseDTO dto)
        {
            if(dto is not SectorDTO sectorDTO)
            {
                return;
            }

            var id = new { this.areaID, sectorDTO.ID };
            dto.Links.Add(new LinkDTO(Url.Link(nameof(GetSectorByID), id), "self", "GET"));
        }
    }
}
