using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogLib.Entities;

namespace PitchLogAPI.Repositories
{
    public interface ISectorsRepository : IRepository<Sector>
    {
        public Task<PagedList<Sector>> GetSectors(int areaID, SectorsResourceParameters parameters);
    }
}
