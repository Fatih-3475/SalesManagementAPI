using System.Linq.Expressions;


namespace SalesManagementAPI.Core.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update (T entity);
        void Delete (T entity);
        Task SaveChangesAsync();
    }
}
