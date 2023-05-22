using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;

namespace PitchLogAPI.Services
{
    public interface IReadRepository<T>
    {
        public Task<bool> Exists(int ID);

        public Task<T> GetByID(int ID);

        public Task<PagedList<T>> GetCollection(BaseResourceParameters parameters);
    }
}
