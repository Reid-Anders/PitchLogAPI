using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;

namespace PitchLogAPI.Repositories
{
    public interface IRoutesRepository : IRepository<PitchLogLib.Entities.Route>
    {
        public Task<bool> Exists(int areaId, int sectorID, string name);

        public Task<PagedList<PitchLogLib.Entities.Route>> GetRoutes(int areaID, RoutesResourceParameters parameters);

        public Task<PagedList<PitchLogLib.Entities.Route>> GetRoutes(int areaID, int sectorID, RoutesResourceParameters parameters);
    }
}
