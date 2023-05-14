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
                source = source.Where(area => area.Municipality.ToLower() == areaResourceParameters.Municipality);
            }

            if (!string.IsNullOrEmpty(areaResourceParameters.Region))
            {
                source = source.Where(area => area.Region.ToLower() == areaResourceParameters.Region);
            }

            if (!string.IsNullOrEmpty(areaResourceParameters.Country))
            {
                source = source.Where(area => area.Country.ToLower() == areaResourceParameters.Country);
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

        public void Delete(int ID)
        {
            throw new NotImplementedException();
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
