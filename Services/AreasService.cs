using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PitchLogAPI.Controllers;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.Repositories;
using PitchLogAPI.ResourceParameters;
using PitchLogLib.Entities;
using System.ComponentModel.DataAnnotations;

namespace PitchLogAPI.Services
{
    public class AreasService : BaseService, IAreasService
    {
        private readonly IAreasRepository _areasRepository;

        public AreasService(IAreasRepository areasRepository, 
            IMapper mapper, 
            IHttpContextAccessor contextAccessor,
            ProblemDetailsFactory problemDetailsFactory) :
            base(mapper, contextAccessor, problemDetailsFactory)
        { 
            _areasRepository = areasRepository ?? throw new ArgumentNullException(nameof(areasRepository));
        }

        public async Task<AreaDTO> GetByID(int ID)
        {
            var area = await _areasRepository.GetByID(ID);

            if(area == null)
            {
                throw new RestException(AreaNotFound(ID));
            }

            var areaToReturn = _mapper.Map<AreaDTO>(area);

            return areaToReturn;
        }

        public async Task<PagedList<AreaDTO>> GetAreas(AreasResourceParameters parameters)
        {
            var areas = await _areasRepository.GetAreas(parameters);
            return _mapper.Map<PagedList<AreaDTO>>(areas);
        }

        public async Task<AreaDTO> CreateArea(AreaForCreationDTO areaForCreation)
        {
            if (await _areasRepository.Exists(areaForCreation.Name))
            {
                throw new RestException(_problemDetailsFactory.CreateProblemDetails(
                    _contextAccessor.HttpContext, 
                    statusCode: 409,
                    detail: $"Area with the name {areaForCreation.Name} already exists."));
            }

            var areaToCreate = _mapper.Map<Area>(areaForCreation);

            _areasRepository.Create(areaToCreate);
            await _areasRepository.SaveChanges();

            return _mapper.Map<AreaDTO>(areaToCreate);
        }

        public async Task<bool> UpdateArea(int ID, AreaForUpdateDTO areaForUpdate)
        {
            var area = await _areasRepository.GetByID(ID);

            if(area == null)
            {
                throw new RestException(AreaNotFound(ID));
            }

            _mapper.Map(areaForUpdate, area);
            return await _areasRepository.SaveChanges();
        }

        public async Task<bool> PatchArea(int ID, JsonPatchDocument<AreaForUpdateDTO> patchDocument, ControllerBase controller)
        {
            var area = await _areasRepository.GetByID(ID);

            if(area == null)
            {
                throw new RestException(AreaNotFound(ID));
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
            return await _areasRepository.SaveChanges();
        }

        public async Task<bool> DeleteArea(int ID)
        {
            var area = await _areasRepository.GetByID(ID);

            if(area == null)
            {
                throw new RestException(AreaNotFound(ID));
            }

            _areasRepository.Delete(area);
            return await _areasRepository.SaveChanges();
        }
    }
}
