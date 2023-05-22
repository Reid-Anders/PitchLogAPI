using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;
using PitchLogAPI.Services;
using PitchLogLib.Entities;

namespace PitchLogAPI.Controllers
{
    [Route("api/areas/{areaID}/sectors")]
    [ApiController]
    public class SectorsController : BasePitchLogController
    {
        private readonly ISectorsRepository _sectorsRepository;
        private readonly IAreasRepository _areasRepository;
        private readonly IMapper _mapper;

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

            var sector = await _sectorsRepository.GetByID(ID);

            var sectorToReturn = _mapper.Map<SectorDTO>(sector);
        }

        [HttpGet(Name = nameof(GetSectors))]
        public async Task<IActionResult> GetSectors(int areaID, [FromQuery] SectorsResourceParameters parameters)
        {

        }

        [HttpPost(Name = nameof(CreateSector))]
        public async Task<IActionResult> CreateSector(int areaID, SectorForCreationDTO sectorForCreation)
        {
            if(!await _areasRepository.Exists(areaID))
            {
                return NotFound();
            }

            var sector = _mapper.Map<Sector>(sectorForCreation);

            _sectorsRepository.Create(sector);
            await _sectorsRepository.Save();

            var sectorToReturn = _mapper.Map<SectorDTO>(sector);

            return CreatedAtRoute(nameof(GetSectorByID), new { areaID, sectorToReturn.ID }, sectorToReturn);
        }

        [HttpPut("{ID}", Name = nameof(UpdateAreaFull))]
        public async Task<IActionResult> UpdateAreaFull(int areaID, int ID, SectorForUpdateDTO areaForUpdate)
        {

        }

        [HttpPatch("{ID}", Name = nameof(UpdateSectorPartial))]
        public async Task<IActionResult> UpdateSectorPartial(int areaID, int ID, JsonPatchDocument<SectorForUpdateDTO> patchDocument)
        {

        }

        [HttpDelete("{ID}", Name = nameof(DeleteSector))]
        public async Task<IActionResult> DeleteSector(int areaID, int ID)
        {

        }

        protected override void AddLinksToResource(LinkedDTO dto)
        {
            if(dto is not SectorDTO sectorDTO)
            {
                return;
            }

            var id = new { sectorDTO.ID };

        }
    }
}
