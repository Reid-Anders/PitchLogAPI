using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.Repositories;
using PitchLogAPI.ResourceParameters;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace PitchLogAPI.Services
{
    public class SectorsService : BaseService, ISectorsService
    {
        private readonly IAreasRepository areasRepoy;
        private readonly ISectorsRepository _sectorsRepo;

        public SectorsService(IAreasRepository areasRepo,
            ISectorsRepository sectorsRepo,
            IMapper mapper,
            IHttpContextAccessor contextAccessor,
            ProblemDetailsFactory problemDetailsFactory) : base(mapper, contextAccessor, problemDetailsFactory)
        {
            _are
            _sectorsRepo = sectorsRepo ?? throw new ArgumentNullException(nameof(sectorsRepo));
        }

        public async Task<SectorDTO> GetByID(int ID)
        {
            if(!await _areas)
            var sector = _sectorsRepo.GetByID(ID);

            if(sector == null)
            {
                throw new RestException(SectorNotFound(ID));
            }

            return _mapper.Map<SectorDTO>(sector);
        }

        public async Task<PagedList<SectorDTO>> GetSectors(int areaID, SectorsResourceParameters parameters)
        {

        }

        public async Task<bool> UpdateSector(int areaID, int ID, SectorForUpdateDTO sectorForUpdate)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> PatchSector(int areaID, int ID, JsonPatchDocument<SectorForUpdateDTO> patchDocument, ControllerBase controller)
        {
            throw new NotImplementedException();
        }

        public async Task<SectorDTO> CreateArea(int areaID, SectorForCreationDTO sectorForCreation)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteSector(int areaID, int ID)
        {
            throw new NotImplementedException();
        }

        private async ProblemDetails SectorNotFound(int ID)
        {
            return _problemDetailsFactory.CreateProblemDetails(
                _contextAccessor.HttpContext,
                statusCode: 400,
                detail: $"Sector with id {ID} not found. Please ensure you have the correct id");
        }
    }
}
