using AutoMapper;
using SalesManagementAPI.Core.DTOs.Customers;
using SalesManagementAPI.Core.DTOs.Orders;
using SalesManagementAPI.Core.DTOs.Products;
using SalesManagementAPI.Core.Entities;

namespace SalesManagementAPI.Business.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<CreateCustomerDto, Customer>();

        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<CreateProductDto, Product>();

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product!.Name));

        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer!.Name))
            .ForMember(dest => dest.Items,
                opt => opt.MapFrom(src => src.OrderItems)); 
    }
}