using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PitchLogAPI.Controllers;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;
using PitchLogData;
using PitchLogLib.Entities;
using System.ComponentModel.DataAnnotations;

namespace PitchLogAPI.Services
{
    public class AreasService : BaseService, IAreasService
    {
        public AreasService(PitchLogContext context, 
            IMapper mapper, 
            IHttpContextAccessor contextAccessor,
            ProblemDetailsFactory problemDetailsFactory) :
            base(context, mapper, contextAccessor, problemDetailsFactory)
        { 

        }

        public async Task<AreaDTO> GetByID(int areaID)
        {
            var area = await _context.Areas.FindAsync(areaID);

            if(area == null)
            {
                throw new RestException(AreaNotFound(areaID));
            }

            var areaToReturn = _mapper.Map<AreaDTO>(area);

            return areaToReturn;
        }

        public async Task<PagedList<AreaDTO>> GetAreas(AreasResourceParameters parameters)
        {
            IQueryable<Area> source = _context.Areas;

            if (!string.IsNullOrEmpty(parameters.Municipality))
            {
                source = source.Where(area => area.Municipality == parameters.Municipality);
            }

            if (!string.IsNullOrEmpty(parameters.Region))
            {
                source = source.Where(area => area.Region == parameters.Region);
            }

            if (!string.IsNullOrEmpty(parameters.Country))
            {
                source = source.Where(area => area.Country == parameters.Country);
            }

            if (!string.IsNullOrEmpty(parameters.SearchQuery))
            {
                source = source.Where(area => area.Name.Contains(parameters.SearchQuery) ||
                    area.Municipality.Contains(parameters.SearchQuery));
            }

            if (!string.IsNullOrEmpty(parameters.OrderBy))
            {
                source = source.ApplySort(parameters.OrderBy);
            }

            var areas = await PagedList<Area>.Create(source, parameters.PageNum, parameters.PageSize);

            return _mapper.Map<PagedList<AreaDTO>>(areas);
        }

        public async Task<AreaDTO> CreateArea(AreaForCreationDTO areaForCreation)
        {
            if (await _context.Areas.AnyAsync(area => area.Name == areaForCreation.Name))
            {
                throw new RestException(_problemDetailsFactory.CreateProblemDetails(
                    _contextAccessor.HttpContext, 
                    statusCode: 409,
                    detail: $"Area with the name {areaForCreation.Name} already exists."));
            }

            var areaToCreate = _mapper.Map<Area>(areaForCreation);

            _context.Areas.Add(areaToCreate);
            await _context.SaveChangesAsync();

            return _mapper.Map<AreaDTO>(areaToCreate);
        }

        public async Task<bool> UpdateArea(int areaID, AreaForUpdateDTO areaForUpdate)
        {
            var area = await _context.Areas.FindAsync(areaID);

            if(area == null)
            {
                throw new RestException(AreaNotFound(areaID));
            }

            _mapper.Map(areaForUpdate, area);
            return await _context.Save();
        }

        public async Task<bool> PatchArea(int areaID, JsonPatchDocument<AreaForUpdateDTO> patchDocument, ControllerBase controller)
        {
            var area = await _context.Areas.FindAsync(areaID);

            if(area == null)
            {
                throw new RestException(AreaNotFound(areaID));
            }

            var areaToPatch = _mapper.Map<AreaForUpdateDTO>(area);
            patchDocument.ApplyTo(areaToPatch);

            if(!controller.TryValidateModel(areaToPatch))
            {
                throw new RestException(_problemDetailsFactory.CreateValidationProblemDetails(
                    _contextAccessor.HttpContext,
                    controller.ModelState));
            }

            _mapper.Map(areaToPatch, area);
            return await _context.Save();
        }

        public async Task<bool> DeleteArea(int areaID)
        {
            var area = await _context.Areas.FindAsync(areaID);

            if(area == null)
            {
                throw new RestException(AreaNotFound(areaID));
            }

            _context.Remove(area);
            return await _context.Save();
        }
    }
}
