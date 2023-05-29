using PitchLogAPI.Model;

namespace PitchLogAPI.Services
{
    public interface IAreasService
    {
        public Task<AreaDTO> GetByID(int ID);
    }
}
