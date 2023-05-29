using PitchLogLib.Entities;

namespace PitchLogAPI.Repositories
{
    public interface IAreasRepository : IRepository<Area>
    {
        public Task<bool> Exists(string name);
    }
}
