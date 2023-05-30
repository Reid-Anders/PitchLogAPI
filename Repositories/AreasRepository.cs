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

        public Task<PagedList<Area>> GetAreas(AreasResourceParameters parameters)
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

            if(!string.IsNullOrEmpty(parameters.SearchQuery))
            {
                source = source.Where(area => area.Name.Contains(parameters.SearchQuery) ||
                    area.Municipality.Contains(parameters.SearchQuery));
            }

            if(!string.IsNullOrEmpty(parameters.OrderBy))
            {
                source = source.ApplySort(parameters.OrderBy);
            }

            return PagedList<Area>.Create(source, parameters.PageNum, parameters.PageSize);
        }
    }
}
