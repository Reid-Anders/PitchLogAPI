using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;

namespace PitchLogAPI.Services
{
    public interface IReadRepository<T>
    {
        public Task<T> GetByID(int ID);

        public Task<PagedList<T>> GetCollection(PaginationResourceParameters parameters);
    }
}
