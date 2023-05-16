namespace PitchLogAPI.Services
{
    public interface ICrudRepository<T> : IReadRepository<T>
    {
        public void Create(T ItemToCreate);

        public void Update(T ItemToUpdate);

        public void Delete(T ItemToDelete);

        public Task<bool> Save();
    }
}
