using AutoMapper;
using SalesManagementAPI.Core.DTOs.Products;
using SalesManagementAPI.Core.Entities;
using SalesManagementAPI.Core.Interfaces.Repositories;
using SalesManagementAPI.Core.Interfaces.Services;
using SalesManagementAPI.Core.Responses;

namespace SalesManagementAPI.Business.Services
{
    public class ProductService : BaseManager, IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<DataResponse<List<ProductDto>>> GetAllAsync()
        {
            var response = new DataResponse<List<ProductDto>>();

            var products = await _productRepository.GetAllAsync();
            response.Data = _mapper.Map<List<ProductDto>>(products);

            return response;
        }
        public async Task<DataResponse<ProductDto>> AddAsync(CreateProductDto dto)
        {
            var response = new DataResponse<ProductDto>();

            var product = _mapper.Map<Product>(dto);

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            response.Data = _mapper.Map<ProductDto>(product);
            return response;
        }
    }
}
