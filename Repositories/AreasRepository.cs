using Microsoft.EntityFrameworkCore;
using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogData;
using PitchLogLib.Entities;

namespace PitchLogAPI.Repositories
{
    public class AreasRepository : BaseRepository<Area>, IAreasRepository
    {
        public AreasRepository(PitchLogContext context) : base(context)
        {

        }

        public override async Task<bool> Exists(int ID)
        {
            return await _context.Areas.AnyAsync(area => area.ID == ID);
        }

        public async Task<bool> Exists(string name)
        {
            return await _context.Areas.AnyAsync(area => area.Name == name);
        }

        public override Task<PagedList<Area>> GetCollection(BaseResourceParameters parameters)
        {
            if(parameters is not AreasResourceParameters areaResourceParameters)
            {
                throw new InvalidOperationException(nameof(parameters));
            }

            IQueryable<Area> source = _context.Areas;

            if (!string.IsNullOrEmpty(areaResourceParameters.Municipality))
            {
                source = source.Where(area => area.Municipality == areaResourceParameters.Municipality);
            }

            if (!string.IsNullOrEmpty(areaResourceParameters.Region))
            {
                source = source.Where(area => area.Region == areaResourceParameters.Region);
            }

            if (!string.IsNullOrEmpty(areaResourceParameters.Country))
            {
                source = source.Where(area => area.Country == areaResourceParameters.Country);
            }

            if(!string.IsNullOrEmpty(areaResourceParameters.SearchQuery))
            {
                source = source.Where(area => area.Name.Contains(areaResourceParameters.SearchQuery) ||
                    area.Municipality.Contains(areaResourceParameters.SearchQuery));
            }

            if(!string.IsNullOrEmpty(areaResourceParameters.OrderBy))
            {
                source = source.ApplySort(areaResourceParameters.OrderBy);
            }

            return PagedList<Area>.Create(source, areaResourceParameters.PageNum, areaResourceParameters.PageSize);
        }
    }
}
