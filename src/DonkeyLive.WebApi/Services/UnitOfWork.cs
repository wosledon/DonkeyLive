using DonkeyLive.WebApi.Data;

namespace DonkeyLive.WebApi.Services
{
    public class UnitOfWork
    {
        private readonly DonkeyDbContext _context;

        public UnitOfWork(DonkeyDbContext context)
        {
            _context = context;
        }

        public DonkeyDbContext Db => _context;

        public IQueryable<T> Query<T>() where T : class
        {
            return _context.Set<T>();
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
