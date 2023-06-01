using PitchLogAPI.Helpers;
using PitchLogAPI.Model;
using PitchLogAPI.ResourceParameters;

namespace PitchLogAPI.Services
{
    public interface IAreasService
    {
        public Task<AreaDTO> GetByID(int ID);

        public Task<PagedList<AreaDTO>> GetAreas(AreasResourceParameters parameters);

        public Task<AreaDTO> CreateArea(AreaForCreationDTO areaToCreate);
    }
}
