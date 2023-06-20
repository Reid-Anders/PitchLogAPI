using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;

namespace PitchLogAPI.Repositories
{
    public interface IRouteRepository : IRepository<PitchLogLib.Entities.Route>
    {
        public Task<bool> Exists(int areaId, int sectorID, string name);

        public Task<PagedList<Route>> GetRoutes(int areaID, int sectorID, RoutesResourceParameters parameters);
    }
}
