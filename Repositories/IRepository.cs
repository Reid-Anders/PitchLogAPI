using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogLib.Entities;

namespace PitchLogAPI.Repositories
{
    public interface IRepository<T> where T : EntityBase
    {
        public Task<bool> Exists(int ID);

        public Task<T> GetByID(int ID);

        public Task<PagedList<T>> GetCollection(BaseResourceParameters parameters);

        public void Create(T ItemToCreate);

        public void Update(T ItemToUpdate);

        public void Delete(T ItemToDelete);

        public Task<bool> SaveChanges();
    }
}
