using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;
using PitchLogData;
using System.Linq.Expressions;

namespace PitchLogAPI.Services
{
    public class RouteService : BaseService, IRouteService
    {
        public RouteService(PitchLogContext context,
            IMapper mapper,
            ProblemDetailsFactory problemDetailsFactory,
            IHttpContextAccessor contextAccessor) : base(context, mapper, contextAccessor, problemDetailsFactory)
        { 

        }

        public async Task<RouteDTO> GetByID(int areaID, int sectorID, int ID)
        {
            var route = await GetRoute(areaID, sectorID, ID);
            return _mapper.Map<RouteDTO>(route);
        }
        public async Task<PagedList<RouteDTO>> GetRoutes(int areaID, RoutesResourceParameters parameters)
        {
            await CheckAreaExists(areaID);

            var routes = await QueryRoutes(_context.Routes.Where(route => route.Sector.AreaID == areaID), parameters);

            return _mapper.Map<PagedList<RouteDTO>>(routes);
        }

        public async Task<PagedList<RouteDTO>> GetRoutes(int areaID, int sectorID, RoutesResourceParameters parameters)
        {
            await CheckAreaAndSector(areaID, sectorID);

            var routes = await QueryRoutes(_context.Routes.Where(route => route.Sector.AreaID == areaID &&
                route.SectorID == sectorID), parameters);

            return _mapper.Map<PagedList<RouteDTO>>(routes);
        }

        public async Task<RouteDTO> CreateRoute(int areaID, int sectorID, RouteForCreationDTO routeForCreation)
        {
            await CheckAreaAndSector(areaID, sectorID);

            var grade = await _context.Grades.FindAsync(routeForCreation.GradeID);

            if(grade == null)
            {
                throw new RestException(_problemDetailsFactory.CreateProblemDetails(
                    _contextAccessor.HttpContext,
                    statusCode: 400,
                    detail: $"Route with id {routeForCreation.GradeID} not found. Please ensure you have the correct id"));
            }

            var route = _mapper.Map<PitchLogLib.Entities.Route>(routeForCreation);
            route.SectorID = sectorID;
            route.Grade = grade;

            _context.Routes.Add(route);
            await _context.Save();

            return _mapper.Map<RouteDTO>(route);
        }
        public async Task<bool> UpdateRoute(int areaID, int sectorID, int ID, RouteForUpdateDTO routeForUpdate)
        {
            var route = await GetRoute(areaID, sectorID, ID);
            _mapper.Map(routeForUpdate, route);

            return await _context.Save();
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
            return await _context.Save();
        }

        public async Task<bool> DeleteRoute(int areaID, int sectorID, int routeID)
        {
            var route = await GetRoute(areaID, sectorID, routeID);

            _context.Remove(route);
            return await _context.Save();
        }

        private async Task<PitchLogLib.Entities.Route> GetRoute(int areaID, int sectorID, int routeID)
        {
            await CheckAreaAndSector(areaID, sectorID);

            var route = await _context.Routes.Include(route => route.Grade).FirstAsync(route => route.ID == routeID);

            if (route == null || route.SectorID != sectorID)
            {
                throw new RestException(RouteNotFound(areaID, sectorID, routeID));
            }

            return route;
        }

        private async Task<bool> CheckAreaExists(int areaID)
        {
            if (!await _context.Areas.AnyAsync(area => area.ID == areaID))
            {
                throw new RestException(AreaNotFound(areaID));
            }

            return true;
        }

        private async Task<bool> CheckAreaAndSector(int areaID, int sectorID)
        {
            await CheckAreaExists(areaID);

            if (!await _context.Sectors.AnyAsync(sector => sector.Area.ID == areaID && sector.ID == sectorID))
            {
                throw new RestException(SectorNotFound(areaID, sectorID));
            }

            return true;
        }

        private async Task<PagedList<PitchLogLib.Entities.Route>> QueryRoutes(IQueryable<PitchLogLib.Entities.Route> source, RoutesResourceParameters parameters)
        {
            if (parameters.Grade?.Count() > 0)
            {
                source = source.Where(route => parameters.Grade.Contains(route.Grade.Value));
            }

            if (!string.IsNullOrEmpty(parameters.SearchQuery))
            {
                source = source.Where(route => route.Name.Contains(parameters.SearchQuery));
            }

            if (!string.IsNullOrEmpty(parameters.OrderBy))
            {
                source = source.ApplySort(parameters.OrderBy);
            }

            source = source.Include(route => route.Grade);

            return await PagedList<PitchLogLib.Entities.Route>.Create(source, parameters.PageNum, parameters.PageSize);
        }
    }
}
