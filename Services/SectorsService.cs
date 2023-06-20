using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.Repositories;
using PitchLogAPI.ResourceParameters;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PitchLogLib.Entities;

namespace PitchLogAPI.Services
{
    public class SectorsService : BaseService, ISectorsService
    {
        private readonly IAreasRepository _areasRepo;
        private readonly ISectorsRepository _sectorsRepo;

        public SectorsService(IAreasRepository areasRepo,
            ISectorsRepository sectorsRepo,
            IMapper mapper,
            IHttpContextAccessor contextAccessor,
            ProblemDetailsFactory problemDetailsFactory) : base(mapper, contextAccessor, problemDetailsFactory)
        {
            _areasRepo = areasRepo ?? throw new ArgumentNullException(nameof(areasRepo));
            _sectorsRepo = sectorsRepo ?? throw new ArgumentNullException(nameof(sectorsRepo));
        }

        public async Task<SectorDTO> GetByID(int areaID, int ID)
        {
            var sector = await GetSector(areaID, ID);
            return _mapper.Map<SectorDTO>(sector);
        }

        public async Task<PagedList<SectorDTO>> GetSectors(int areaID, SectorsResourceParameters parameters)
        {
            if(!await _areasRepo.Exists(areaID))
            {
                throw new RestException(AreaNotFound(areaID));
            }

            var sectors = await _sectorsRepo.GetSectors(areaID, parameters);
            return _mapper.Map<PagedList<SectorDTO>>(sectors);
        }
        public async Task<SectorDTO> CreateSector(int areaID, SectorForCreationDTO sectorForCreation)
        {
            if(!await _areasRepo.Exists(areaID))
            {
                throw new RestException(AreaNotFound(areaID));
            }

            if(await _sectorsRepo.Exists(areaID, sectorForCreation.Name))
            {
                throw new RestException(_problemDetailsFactory.CreateProblemDetails(
                    _contextAccessor.HttpContext,
                    statusCode: 409,
                    title: "Sector already exists",
                    detail: $"A sector with the name {sectorForCreation.Name} already exists",
                    instance: _contextAccessor.HttpContext.Request.GetAbsoluteUri()));
            }

            var sector = _mapper.Map<Sector>(sectorForCreation);
            sector.AreaID = areaID;

            _sectorsRepo.Create(sector);
            await _sectorsRepo.SaveChanges();

            return _mapper.Map<SectorDTO>(sector);
        }

        public async Task<bool> UpdateSector(int areaID, int ID, SectorForUpdateDTO sectorForUpdate)
        {
            var sector = await GetSector(areaID, ID);

            _mapper.Map(sectorForUpdate, sector);
            return await _sectorsRepo.SaveChanges();
        }

        public async Task<bool> PatchSector(int areaID, int ID, JsonPatchDocument<SectorForUpdateDTO> patchDocument, ControllerBase controller)
        {
            var sector = await GetSector(areaID, ID);

            var sectorToPatch = _mapper.Map<SectorForUpdateDTO>(sector);
            patchDocument.ApplyTo(sectorToPatch);

            if(!controller.TryValidateModel(sectorToPatch))
            {
                throw new RestException(_problemDetailsFactory.CreateValidationProblemDetails(
                    _contextAccessor.HttpContext,
                    controller.ModelState));
            }

            _mapper.Map(sectorToPatch, sector);
            return await _sectorsRepo.SaveChanges();
        }


        public async Task<bool> DeleteSector(int areaID, int ID)
        {
            var sector = await GetSector(areaID, ID);

            _sectorsRepo.Delete(sector);
            return await _sectorsRepo.SaveChanges();
        }

        private async Task<Sector> GetSector(int areaID, int ID)
        {
            if (!await _areasRepo.Exists(areaID))
            {
                throw new RestException(AreaNotFound(areaID));
            }

            var sector = await _sectorsRepo.GetByID(ID);

            if (sector == null || sector.AreaID != areaID)
            {
                throw new RestException(SectorNotFound(areaID, ID));
            }

            return sector;
        }
    }
}
