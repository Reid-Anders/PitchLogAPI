using Microsoft.EntityFrameworkCore;
using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogData;
using PitchLogLib.Entities;

namespace PitchLogAPI.Services
{
    public class AreasRepository : BaseRepository, IAreasRepository
    {
        public AreasRepository(PitchLogContext context) : base(context)
        {

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

        public async Task<PagedList<Area>> GetCollection(BaseResourceParameters parameters)
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

            return await PagedList<Area>.Create(source, areaResourceParameters.PageNum, areaResourceParameters.PageSize);
        }

        public void Create(Area ItemToCreate)
        {
            if (ItemToCreate == null)
            {
                throw new ArgumentNullException(nameof(ItemToCreate));
            }

            _context.Areas.Add(ItemToCreate);
        }

        public void Delete(Area ItemToDelete)
        {
            if(ItemToDelete == null)
            {
                throw new ArgumentNullException(nameof(ItemToDelete));
            }

            _context.Areas.Remove(ItemToDelete);
        }

        public void Update(Area ItemToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
