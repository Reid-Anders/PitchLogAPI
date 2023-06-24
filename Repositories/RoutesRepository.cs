using Microsoft.EntityFrameworkCore;
using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogData;

namespace PitchLogAPI.Repositories
{
    public class RoutesRepository : BaseRepository<PitchLogLib.Entities.Route>, IRoutesRepository
    {
        public RoutesRepository(PitchLogContext context) : base(context)
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

        public override async Task<PitchLogLib.Entities.Route> GetByID(int ID)
        {
            return await _context.Routes.Where(route => route.ID == ID).Include(route => route.Grade).FirstOrDefaultAsync();
        }

        public async Task<PagedList<PitchLogLib.Entities.Route>> GetRoutes(int areaID, RoutesResourceParameters parameters)
        {
            var source = _context.Routes.Where(route => route.Sector.AreaID == areaID);
            return await GetRoutes(source, parameters);
        }

        public async Task<PagedList<PitchLogLib.Entities.Route>> GetRoutes(int areaID, int sectorID, RoutesResourceParameters parameters)
        {
            var source = _context.Routes.Where(route => route.SectorID == sectorID && route.Sector.AreaID == areaID);
            return await GetRoutes(source, parameters);
        }

        private async Task<PagedList<PitchLogLib.Entities.Route>> GetRoutes(IQueryable<PitchLogLib.Entities.Route> source, RoutesResourceParameters parameters)
        {
            if (parameters.Grade?.Count() > 0)
            {
                //may need to be a lot more specific
                source = source.ApplyComparisonFilter("grade", parameters.Grade);
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
