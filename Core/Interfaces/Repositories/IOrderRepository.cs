using SalesManagementAPI.Core.Entities;


namespace SalesManagementAPI.Core.Interfaces.Repositories
{
    public interface IOrderRepository :IGenericRepository<Order>
    {
        Task<List<Order>> GetAllWithDetailsAsync();
        Task<Order> GetByIdWithDetailsAsync(int id);
        IQueryable<Order> Query();
    }
}
