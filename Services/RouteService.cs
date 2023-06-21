using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.Repositories;
using PitchLogAPI.ResourceParameters;

namespace PitchLogAPI.Services
{
    public class RouteService : BaseService, IRouteService
    {
        private readonly IRoutesRepository _routeRepo;
        private readonly ISectorsRepository _sectorRepository;
        private readonly IAreasRepository _areaRepository;

        public RouteService(IRoutesRepository routeRepo,
            ISectorsRepository sectorRepository,
            IAreasRepository areasRepository,
            IMapper mapper,
            ProblemDetailsFactory problemDetailsFactory,
            IHttpContextAccessor contextAccessor) : base(mapper, contextAccessor, problemDetailsFactory)
        { 
            _routeRepo = routeRepo ?? throw new ArgumentNullException(nameof(routeRepo));
            _sectorRepository = sectorRepository ?? throw new ArgumentNullException(nameof(sectorRepository));
            _areaRepository = areasRepository ?? throw new ArgumentNullException(nameof(sectorRepository));
        }
        public async Task<RouteDTO> GetByID(int areaID, int sectorID, int ID)
        {
            var route = GetRoute(areaID, sectorID, ID);
            return _mapper.Map<RouteDTO>(route);
        }
        public async Task<PagedList<RouteDTO>> GetRoutes(int areaID, RoutesResourceParameters parameters)
        {
            if (!await _areaRepository.Exists(areaID))
            {
                throw new RestException(AreaNotFound(areaID));
            }

            var routes = await _routeRepo.GetRoutes(areaID, parameters);

            return _mapper.Map<PagedList<RouteDTO>>(routes);
        }

        public async Task<PagedList<RouteDTO>> GetRoutes(int areaID, int sectorID, RoutesResourceParameters parameters)
        {
            await CheckAreaAndSector(areaID, sectorID);

            var routes = await _routeRepo.GetRoutes(areaID, sectorID, parameters);

            return _mapper.Map<PagedList<RouteDTO>>(routes);
        }

        public async Task<RouteDTO> CreateRoute(int areaID, int sectorID, RouteForCreationDTO routeForCreation)
        {
            await CheckAreaAndSector(areaID, sectorID);

            var route = _mapper.Map<PitchLogLib.Entities.Route>(routeForCreation);
            route.SectorID = sectorID;

            _routeRepo.Create(route);
            await _routeRepo.SaveChanges();

            return _mapper.Map<RouteDTO>(route);
        }
        public async Task<bool> UpdateRoute(int areaID, int sectorID, int ID, RouteForUpdateDTO routeForUpdate)
        {
            var route = await GetRoute(areaID, sectorID, ID);
            _mapper.Map(routeForUpdate, route);

            return await _routeRepo.SaveChanges();
        }

        public async Task<bool> PatchRoute(int areaID, int sectorID, int ID, JsonPatchDocument<RouteForUpdateDTO> patchDocument, ControllerBase controller)
        {
            var route = await GetRoute(areaID, sectorID, ID);

            var routeToPatch = _mapper.Map<RouteForUpdateDTO>(route);
            patchDocument.ApplyTo(routeToPatch);

            if(!controller.TryValidateModel(routeToPatch))
            {
                throw new RestException(_problemDetailsFactory.CreateValidationProblemDetails(
                    _contextAccessor.HttpContext,
                    controller.ModelState));
            }

            _mapper.Map(routeToPatch, route);
            return await _routeRepo.SaveChanges();
        }

        public async Task<bool> DeleteRoute(int areaID, int sectorID, int ID)
        {
            var route = await GetRoute(areaID, sectorID, ID);

            _routeRepo.Delete(route);
            return await _routeRepo.SaveChanges();
        }

        private async Task<PitchLogLib.Entities.Route> GetRoute(int areaID, int sectorID, int ID)
        {
            await CheckAreaAndSector(areaID, sectorID);

            var route = await _routeRepo.GetByID(ID);

            if (route == null || route.SectorID != sectorID || route.Sector.AreaID != areaID)
            {
                throw new RestException(RouteNotFound(areaID, sectorID, ID));
            }

            return route;
        }

        private async Task<bool> CheckAreaAndSector(int areaID, int sectorID)
        {
            if (!await _areaRepository.Exists(areaID))
            {
                throw new RestException(AreaNotFound(areaID));
            }

            if (!await _sectorRepository.Exists(sectorID))
            {
                throw new RestException(SectorNotFound(areaID, sectorID));
            }

            return true;
        }
    }
}
