using Microsoft.EntityFrameworkCore;
using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogData;
using PitchLogLib.Entities;

namespace PitchLogAPI.Services
{
    public class AreasRepository : IAreasRepository
    {
        private readonly PitchLogContext _context;

        public AreasRepository(PitchLogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> Exists(int ID)
        {
            return await _context.Areas.AnyAsync(area => area.ID == ID);
        }

        public async Task<bool> Exists(string name)
        {
            return await _context.Areas.AnyAsync(area => area.Name == name);
        }

        public async Task<Area> GetByID(int ID)
        {
            return await _context.Areas.FindAsync(ID);
        }

        public async Task<PagedList<Area>> GetCollection(PaginationResourceParameters parameters)
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
                source = source.Sort(areaResourceParameters.OrderBy);
            }

            return await PagedList<Area>.Create(source, areaResourceParameters.PageNum, areaResourceParameters.PageSize);
        }

        public void Create(Area area)
        {
            if (area == null)
            {
                throw new ArgumentNullException(nameof(area));
            }

            _context.Areas.Add(area);
        }

        public void Delete(Area ItemToDelete)
        {
            _context.Areas.Remove(ItemToDelete);
        }

        public void Update(Area ItemToUpdate)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
