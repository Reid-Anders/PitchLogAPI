using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogLib.Entities;

namespace PitchLogAPI.Repositories
{
    public interface IAreasRepository : IRepository<Area>
    {
        public Task<bool> Exists(string name);

        public Task<PagedList<Area>> GetAreas(AreasResourceParameters parameters);
    }
}
