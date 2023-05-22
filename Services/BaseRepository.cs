using PitchLogData;

namespace PitchLogAPI.Services
{
    public abstract class BaseRepository
    { 
        protected readonly PitchLogContext _context;

        public BaseRepository(PitchLogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
