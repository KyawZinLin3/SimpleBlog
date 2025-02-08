using SimpleBlog.WebAPI.Repositories.Base;

namespace SimpleBlog.WebAPI.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
}
