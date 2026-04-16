using SalesManagementAPI.Core.Responses;
using SalesManagementAPI.Core.DTOs;
using SalesManagementAPI.Core.DTOs.Orders;

namespace SalesManagementAPI.Core.Interfaces.Services
{
    public interface IReportService
    {
        Task<DataResponse<SalesReportDto>> GetSalesReportAsync();
        Task<DataResponse<List<OrderDto>>> FilterOrdersAsync(
            DateTime? startDate,
            DateTime? endDate,
            decimal? minAmount,
            decimal? maxAmount,
            int? customerId
            );
   
    }
}
