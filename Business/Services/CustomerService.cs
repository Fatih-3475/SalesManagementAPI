using AutoMapper;
using SalesManagementAPI.Core.DTOs.Customers;
using SalesManagementAPI.Core.Entities;
using SalesManagementAPI.Core.Interfaces.Repositories;
using SalesManagementAPI.Core.Interfaces.Services;
using SalesManagementAPI.Core.Responses;

namespace SalesManagementAPI.Business.Services
{
    public class CustomerService : BaseManager, ICustomerService
    {
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(IGenericRepository<Customer> customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<DataResponse<List<CustomerDto>>> GetAllAsync()
        {
            var response = new DataResponse<List<CustomerDto>>();

            var customers = await _customerRepository.GetAllAsync();
            response.Data = _mapper.Map<List<CustomerDto>>(customers);

            return response;
        }

        public async Task<DataResponse<CustomerDto>> AddAsync(CreateCustomerDto dto)
        {
            var response = new DataResponse<CustomerDto>();

            var customer = _mapper.Map<Customer>(dto);
            customer.CreatedDate = DateTime.UtcNow;

            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();

            response.Data = _mapper.Map<CustomerDto>(customer);

            return response;
        }
    }
}