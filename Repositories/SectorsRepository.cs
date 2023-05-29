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

        public override Task<PagedList<Sector>> GetCollection(BaseResourceParameters parameters)
        {
            if(parameters is not SectorsResourceParameters sectorsResourceParameters)
            {
                throw new InvalidOperationException(nameof(parameters));
            }

            IQueryable<Sector> source = _context.Sectors;

            if(sectorsResourceParameters.Approach.Count() > 0)
            {
                source = source.ApplyComparisonFilter("Approach", sectorsResourceParameters.Approach);
            }

            if(!string.IsNullOrEmpty(sectorsResourceParameters.Aspect))
            {
                source = source.Where(sector => sector.Aspect.ToString() == sectorsResourceParameters.Aspect);
            }

            if(!string.IsNullOrEmpty(sectorsResourceParameters.SearchQuery))
            {
                source = source.Where(sector => sector.Name.Contains(sectorsResourceParameters.SearchQuery) ||
                    sector.Area.Name.Contains(sectorsResourceParameters.SearchQuery) ||
                    sector.Area.Municipality.Contains(sectorsResourceParameters.SearchQuery));
            }

            if(!string.IsNullOrEmpty(sectorsResourceParameters.OrderBy))
            {
                source = source.ApplySort(sectorsResourceParameters.OrderBy);
            }

            return PagedList<Sector>.Create(source, sectorsResourceParameters.PageNum, sectorsResourceParameters.PageSize);
        }
    }
}
