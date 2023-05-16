using PitchLogLib.Entities;

namespace PitchLogAPI.Services
{
    public interface IAreasRepository : ICrudRepository<Area>
    {
        public Task<bool> Exists(string name);
    }
}
