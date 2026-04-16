using SalesManagementAPI.Core.DTOs.Products;
using SalesManagementAPI.Core.Responses;


namespace SalesManagementAPI.Core.Interfaces.Services
{
    public interface IProductService
    {
        Task<DataResponse<List<ProductDto>>> GetAllAsync();
        Task<DataResponse<ProductDto>> AddAsync(CreateProductDto dto);
    }
}
