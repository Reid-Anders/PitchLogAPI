namespace PitchLogAPI.Services
{
    public interface ICrudRepository<T> : IReadRepository<T>
    {
        public void Create(T ItemToCreate);

        public void Update(T ItemToUpdate);

        public void Delete(int ID);

        public Task<bool> Save();
    }
}
