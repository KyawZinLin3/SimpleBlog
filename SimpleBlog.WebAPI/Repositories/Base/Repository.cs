using Microsoft.EntityFrameworkCore;
using SimpleBlog.WebAPI.Persistence;

namespace SimpleBlog.WebAPI.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly SimpleBlogDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(SimpleBlogDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    }
}
