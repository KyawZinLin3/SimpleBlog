using Microsoft.EntityFrameworkCore;
using SimpleBlog.WebAPI.Persistence;
using SimpleBlog.WebAPI.Repositories.Base;

namespace SimpleBlog.WebAPI.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SimpleBlogDbContext _context;

        public UnitOfWork(SimpleBlogDbContext context)
        {
            _context = context;
        }

        public IRepository<T> Repository<T>() where T : class => new Repository<T>(_context);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    }
}
