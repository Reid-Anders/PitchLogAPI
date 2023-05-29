using PitchLogAPI.Helpers;
using PitchLogAPI.ResourceParameters;
using PitchLogData;
using PitchLogLib.Entities;
using Microsoft.EntityFrameworkCore;

namespace PitchLogAPI.Repositories
{
    public abstract class BaseRepository<T> where T : EntityBase
    { 
        protected readonly PitchLogContext _context;

        public BaseRepository(PitchLogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
        public virtual async Task<T> GetByID(int ID)
        {
            return await _context.FindAsync<T>(ID);
        }

        public virtual void Create(T ItemToCreate)
        {
            if (ItemToCreate == null)
            {
                throw new ArgumentNullException(nameof(ItemToCreate));
            }

            _context.Add(ItemToCreate);
        }

        public virtual void Update(T ItemToUpdate)
        {
            if(ItemToUpdate == null)
            {
                throw new ArgumentNullException(nameof(ItemToUpdate));
            }

            _context.Update(ItemToUpdate);
        }

        public virtual void Delete(T ItemToDelete)
        {
            if(ItemToDelete == null)
            {
                throw new ArgumentNullException(nameof(ItemToDelete));
            }

            _context.Remove(ItemToDelete);
        }

        public abstract Task<bool> Exists(int ID);

        public abstract Task<PagedList<T>> GetCollection(BaseResourceParameters parameters);
    }
}
