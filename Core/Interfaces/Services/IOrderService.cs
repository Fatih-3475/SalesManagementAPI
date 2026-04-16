using SalesManagementAPI.Core.DTOs.Orders;
using SalesManagementAPI.Core.Responses;


namespace SalesManagementAPI.Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<DataResponse<List<OrderDto>>> GetAllAsync();
        Task<DataResponse<OrderDto>> CreateOrderAsync(CreateOrderDto dto);

    }
}
