using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PitchLogLib.Entities;
using Microsoft.EntityFrameworkCore;
using PitchLogData;

namespace PitchLogAPI.Services
{
    public class SectorsService : BaseService, ISectorsService
    {
        public SectorsService(PitchLogContext context,
            IMapper mapper,
            IHttpContextAccessor contextAccessor,
            ProblemDetailsFactory problemDetailsFactory) : base(context, mapper, contextAccessor, problemDetailsFactory)
        {

        }

        public async Task<SectorDTO> GetByID(int areaID, int ID)
        {
            var sector = await GetSector(areaID, ID);
            return _mapper.Map<SectorDTO>(sector);
        }

        public async Task<PagedList<SectorDTO>> GetSectors(int areaID, SectorsResourceParameters parameters)
        {
            await CheckAreaExists(areaID);

            IQueryable<Sector> source = _context.Sectors.Where(sector => sector.AreaID == areaID);

            if (parameters.Approach?.Count() > 0)
            {
                source = source.ApplyComparisonFilter("Approach", parameters.Approach);
            }

            if (!string.IsNullOrEmpty(parameters.Aspect))
            {
                source = source.Where(sector => sector.Aspect.ToString() == parameters.Aspect);
            }

            if (!string.IsNullOrEmpty(parameters.SearchQuery))
            {
                source = source.Where(sector => sector.Name.Contains(parameters.SearchQuery) ||
                    sector.Area.Name.Contains(parameters.SearchQuery) ||
                    sector.Area.Municipality.Contains(parameters.SearchQuery));
            }

            if (!string.IsNullOrEmpty(parameters.OrderBy))
            {
                source = source.ApplySort(parameters.OrderBy);
            }

            var sectors = await PagedList<Sector>.Create(source, parameters.PageNum, parameters.PageSize);

            return _mapper.Map<PagedList<SectorDTO>>(sectors);
        }
        public async Task<SectorDTO> CreateSector(int areaID, SectorForCreationDTO sectorForCreation)
        {
            await CheckAreaExists(areaID);

            if(await _context.Sectors.AnyAsync(sector => sector.AreaID == areaID && sector.Name == sectorForCreation.Name))
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

            _context.Sectors.Add(sector);
            await _context.Save();

            return _mapper.Map<SectorDTO>(sector);
        }

        public async Task<bool> UpdateSector(int areaID, int ID, SectorForUpdateDTO sectorForUpdate)
        {
            var sector = await GetSector(areaID, ID);

            _mapper.Map(sectorForUpdate, sector);
            return await _context.Save();
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
            return await _context.Save();
        }


        public async Task<bool> DeleteSector(int areaID, int ID)
        {
            var sector = await GetSector(areaID, ID);

            _context.Sectors.Remove(sector);
            return await _context.Save();
        }

        private async Task<bool> CheckAreaExists(int areaID)
        {
            if(!await _context.Areas.AnyAsync(area => area.ID == areaID)) 
            {
                throw new RestException(AreaNotFound(areaID));
            }

            return true;
        }

        private async Task<Sector> GetSector(int areaID, int sectorID)
        {
            await CheckAreaExists(areaID);

            var sector = await _context.Sectors.FindAsync(sectorID);

            if (sector == null || sector.AreaID != areaID)
            {
                throw new RestException(SectorNotFound(areaID, sectorID));
            }

            return sector;
        }
    }
}
