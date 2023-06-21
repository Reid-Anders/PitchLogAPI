using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;

namespace PitchLogAPI.Services
{
    public interface IRouteService
    {
        public Task<RouteDTO> GetByID(int areaID, int sectorID, int ID);

        public Task<PagedList<RouteDTO>> GetRoutes(int areaID, RoutesResourceParameters parameters);

        public Task<PagedList<RouteDTO>> GetRoutes(int areaID, int sectorID, RoutesResourceParameters parameters);

        public Task<RouteDTO> CreateRoute(int areaID, int sectorID, RouteForCreationDTO routeForCreation);

        public Task<bool> UpdateRoute(int areaID, int sectorID, int ID, RouteForUpdateDTO routeForUpdate);

        public Task<bool> PatchRoute(int areaID, int sectorID, int ID, JsonPatchDocument<RouteForUpdateDTO> patchDocument, ControllerBase controller);

        public Task<bool> DeleteRoute(int areaID, int sectorID, int ID);
    }
}
