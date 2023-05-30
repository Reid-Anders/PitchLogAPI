using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PitchLogAPI.Controllers;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.Repositories;
using PitchLogAPI.ResourceParameters;

namespace PitchLogAPI.Services
{
    public class AreasService : BaseService, IAreasService
    {
        private readonly IAreasRepository _areasRepository;

        public AreasService(IAreasRepository areasRepository, 
            IMapper mapper, 
            IHttpContextAccessor contextAccessor,
            ProblemDetailsFactory problemDetailsFactory,
            LinkGenerator linkGenerator) :
            base(mapper, contextAccessor, problemDetailsFactory, linkGenerator)
        { 
            _areasRepository = areasRepository ?? throw new ArgumentNullException(nameof(areasRepository));
        }

        public async Task<AreaDTO> GetByID(int ID)
        {
            var area = await _areasRepository.GetByID(ID);

            if(area == null)
            {
                throw new ResourceNotFoundException($"Area with id {ID} not found.");
            }

            var areaToReturn = _mapper.Map<AreaDTO>(area);
            LinkResource(areaToReturn);

            return areaToReturn;
        }

        public async Task<PagedList<AreaDTO>> GetAreas(AreasResourceParameters parameters)
        {
            var areas = await _areasRepository.GetAreas(parameters);
            return _mapper.Map<PagedList<AreaDTO>>(areas);
        }

        public override void LinkResource(BaseDTO resource)
        {
            if (resource is not AreaDTO area)
            {
                return;
            }

            var id = new { area.ID };
            area.Links.Add(new LinkDTO(_linkGenerator.GetPathByName(_contextAccessor.HttpContext, nameof(AreasController.GetAreaByID), id), "self", "GET"));
            //area.Links.Add(new LinkDTO(Url.Link(nameof(CreateArea), new { }), "create", "POST"));
            //area.Links.Add(new LinkDTO(Url.Link(nameof(UpdateAreaFull), id), "update", "PUT"));
            //area.Links.Add(new LinkDTO(Url.Link(nameof(UpdateAreaPartial), id), "update_partial", "PATCH"));
            //area.Links.Add(new LinkDTO(Url.Link(nameof(DeleteArea), id), "delete", "DELETE"));
            area.Links.Add(new LinkDTO(_linkGenerator.GetPathByName(_contextAccessor.HttpContext, nameof(SectorsController.GetSectors), new { areaID = area.ID }), "sectors", "GET"));
        }
    }
}
