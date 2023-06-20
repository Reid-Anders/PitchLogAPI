using Microsoft.EntityFrameworkCore;
using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogData;

namespace PitchLogAPI.Repositories
{
    public class RouteRepository : BaseRepository<PitchLogLib.Entities.Route>, IRouteRepository
    {
        public RouteRepository(PitchLogContext context) : base(context)
        {

        }

        public override async Task<bool> Exists(int ID)
        {
            return await _context.Routes.AnyAsync(route => route.ID == ID);
        }

        public async Task<bool> Exists(int areaID, int sectorID, string name)
        {
            return await _context.Routes.AnyAsync(route =>
                route.Name == name && route.Sector.Area.ID == areaID && route.SectorID == sectorID);
        }

        public async Task<PagedList<PitchLogLib.Entities.Route>> GetRoutes(int areaID, int sectorID, RoutesResourceParameters parameters)
        {
            var source = _context.Routes.Where(route => route.SectorID == sectorID);

            if(parameters.Grade?.Count() > 0)
            {
                source = source.ApplyComparisonFilter("grade", parameters.Grade);
            }

            if(!string.IsNullOrEmpty(parameters.SearchQuery))
            {
                source = source.Where(route => route.Name.Contains(parameters.SearchQuery));
            }

            if(!string.IsNullOrEmpty(parameters.OrderBy))
            {
                source = source.ApplySort(parameters.OrderBy);
            }

            return await PagedList<PitchLogLib.Entities.Route>.Create(source, parameters.PageNum, parameters.PageSize);
        }
    }
}
