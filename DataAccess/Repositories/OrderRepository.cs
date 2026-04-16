using Microsoft.EntityFrameworkCore;
using SalesManagementAPI.Core.Entities;
using SalesManagementAPI.Core.Interfaces.Repositories;
using SalesManagementAPI.DataAccess.Contexts;

namespace SalesManagementAPI.DataAccess.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    private readonly SalesManagementDbContext _context;

    public OrderRepository(SalesManagementDbContext context) : base(context)
    {
        
        _context = context;
    }

    public async Task<List<Order>> GetAllWithDetailsAsync()
    {
        return await _context.Orders
            .AsNoTracking() 
            .Include(x => x.Customer)
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Orders
            .Include(x => x.Customer)
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<Order> Query()
    {
        return _context.Orders
            .Include(x => x.Customer)
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
            .AsQueryable();
    }
}