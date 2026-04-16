using SalesManagementAPI.Core.DTOs.Customers;
using SalesManagementAPI.Core.Responses;

namespace SalesManagementAPI.Core.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<DataResponse<List<CustomerDto>>> GetAllAsync();
        Task<DataResponse<CustomerDto>> AddAsync(CreateCustomerDto dto);
    }
}
