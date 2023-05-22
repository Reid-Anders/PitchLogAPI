using Microsoft.EntityFrameworkCore;
using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogData;
using PitchLogLib.Entities;

namespace PitchLogAPI.Services
{
    public class SectorsRepository : BaseRepository, ISectorsRepository
    {
        public SectorsRepository(PitchLogContext context) : base(context)
        {

        }

        public async Task<Sector> GetByID(int ID)
        {
            return await _context.Sectors.FindAsync(ID);
        }

        public Task<PagedList<Sector>> GetCollection(BaseResourceParameters parameters)
        {
            if(parameters is not SectorsResourceParameters sectorsResourceParameters)
            {
                throw new InvalidOperationException(nameof(parameters));
            }

            IQueryable<Sector> source = _context.Sectors;

            if(!string.IsNullOrEmpty(sectorsResourceParameters.Name))
            {
                source.Where(sector => sector.Name == sectorsResourceParameters.Name);
            }

            if(sectorsResourceParameters.Approach != null)
            {
                source.Where(sector => sector.Approach == sectorsResourceParameters.Approach);
            }

            if(!string.IsNullOrEmpty(sectorsResourceParameters.Aspect))
            {
                source.Where(sector => sector.Aspect.ToString() == sectorsResourceParameters.Aspect);
            }

            if(!string.IsNullOrEmpty(sectorsResourceParameters.SearchQuery))
            {
                source.Where(sector => sector.Name.Contains(sectorsResourceParameters.SearchQuery) ||
                    sector.Area.Name.Contains(sectorsResourceParameters.SearchQuery) ||
                    sector.Area.Municipality.Contains(sectorsResourceParameters.SearchQuery));
            }

            if(!string.IsNullOrEmpty(sectorsResourceParameters.OrderBy))
            {
                source.Sort(sectorsResourceParameters.OrderBy);
            }

            return PagedList<Sector>.Create(source, sectorsResourceParameters.PageNum, sectorsResourceParameters.PageSize);
        }

        public void Create(Sector ItemToCreate)
        {
            if(ItemToCreate == null)
            {
                throw new ArgumentNullException(nameof(ItemToCreate));
            }

            _context.Add(ItemToCreate);
        }

        public void Delete(Sector ItemToDelete)
        {
            if (ItemToDelete == null)
            {
                throw new ArgumentNullException(nameof(ItemToDelete));
            }

            _context.Add(ItemToDelete);
        }

        public async Task<bool> Exists(int ID)
        {
            return await _context.Sectors.AnyAsync(sector => sector.ID == ID);
        }


        public void Update(Sector ItemToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
