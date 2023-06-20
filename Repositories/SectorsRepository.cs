using Microsoft.EntityFrameworkCore;
using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogData;
using PitchLogLib.Entities;

namespace PitchLogAPI.Repositories
{
    public class SectorsRepository : BaseRepository<Sector>, ISectorsRepository
    {
        public SectorsRepository(PitchLogContext context) : base(context)
        {

        }

        public override async Task<bool> Exists(int ID)
        {
            return await _context.Sectors.AnyAsync(sector => sector.ID == ID);
        }

        public async Task<bool> Exists(int areaID, string name)
        {
            return await _context.Sectors.AnyAsync(sector => sector.Name == name && sector.AreaID == areaID);
        }

        public Task<PagedList<Sector>> GetSectors(int areaID, SectorsResourceParameters parameters)
        {
            IQueryable<Sector> source = _context.Sectors.Where(sector => sector.AreaID == areaID);

            if(parameters.Approach?.Count() > 0)
            {
                source = source.ApplyComparisonFilter("Approach", parameters.Approach);
            }

            if(!string.IsNullOrEmpty(parameters.Aspect))
            {
                source = source.Where(sector => sector.Aspect.ToString() == parameters.Aspect);
            }

            if(!string.IsNullOrEmpty(parameters.SearchQuery))
            {
                source = source.Where(sector => sector.Name.Contains(parameters.SearchQuery) ||
                    sector.Area.Name.Contains(parameters.SearchQuery) ||
                    sector.Area.Municipality.Contains(parameters.SearchQuery));
            }

            if(!string.IsNullOrEmpty(parameters.OrderBy))
            {
                source = source.ApplySort(parameters.OrderBy);
            }

            return PagedList<Sector>.Create(source, parameters.PageNum, parameters.PageSize);
        }
    }
}
