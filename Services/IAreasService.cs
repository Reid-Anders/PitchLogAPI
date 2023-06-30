using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;

namespace PitchLogAPI.Services
{
    public interface IAreasService
    {
        public Task<AreaDTO> GetByID(int ID);

        public Task<PagedList<AreaDTO>> GetAreas(AreasResourceParameters parameters);

        public Task<AreaDTO> CreateArea(AreaForCreationDTO areaForCreation);

        public Task<bool> UpdateArea(int ID, AreaForUpdateDTO areaForUpdate);

        public Task<bool> PatchArea(int ID, JsonPatchDocument<AreaForUpdateDTO> patchDocument, ControllerBase controller);

        public Task<bool> DeleteArea(int ID);

        public Task<bool> AnySectors(int areaID);

        public Task<bool> AnyRoutes(int areaID);
    }
}
