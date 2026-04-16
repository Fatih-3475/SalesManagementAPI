using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesManagementAPI.Core.Constants;
using SalesManagementAPI.Core.DTOs.Orders;
using SalesManagementAPI.Core.Entities;
using SalesManagementAPI.Core.Interfaces.Repositories;
using SalesManagementAPI.Core.Interfaces.Services;
using SalesManagementAPI.Core.Responses;
using SalesManagementAPI.DataAccess.Contexts;

namespace SalesManagementAPI.Business.Services;

public class OrderService : BaseManager, IOrderService
{
    private readonly IGenericRepository<Customer> _customerRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly SalesManagementDbContext _context;
    private readonly IMapper _mapper;

    public OrderService(
        IGenericRepository<Customer> customerRepository,
        IOrderRepository orderRepository,
        SalesManagementDbContext context,
        IMapper mapper)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
        _context = context;
        _mapper = mapper;
    }

    public async Task<DataResponse<List<OrderDto>>> GetAllAsync()
    {
        var response = new DataResponse<List<OrderDto>>();

        var orders = await _orderRepository.GetAllWithDetailsAsync();
        response.Data = _mapper.Map<List<OrderDto>>(orders);

        return response;
    }

    public async Task<DataResponse<OrderDto>> CreateOrderAsync(CreateOrderDto dto)
    {
        var response = new DataResponse<OrderDto>();


        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
        if (customer is null)
        {
            AddError(response, opt =>
            {
                opt.ErrorCode = ErrorCodeValues.CustomerNotFound;
                opt.ErrorMessage = "Müşteri bulunamadı.";
                opt.PropertyName = nameof(dto.CustomerId);
                opt.AttemptedValue = dto.CustomerId;
            });

            return response;
        }


        if (dto.Items is null || !dto.Items.Any())
        {
            AddError(response, opt =>
            {
                opt.ErrorCode = ErrorCodeValues.InvalidOrder;
                opt.ErrorMessage = "Sipariş en az bir ürün içermelidir.";
                opt.PropertyName = nameof(dto.Items);
                opt.AttemptedValue = dto.Items;
            });

            return response;
        }

        var productIds = dto.Items
            .Select(x => x.ProductId)
            .Distinct()
            .ToList();

        var products = await _context.Products
            .Where(x => productIds.Contains(x.Id))
            .ToListAsync();

        if (products.Count != productIds.Count)
        {
            AddError(response, opt =>
            {
                opt.ErrorCode = ErrorCodeValues.ProductNotFound;
                opt.ErrorMessage = "Siparişte bulunan ürünlerden biri veya birkaçı sistemde bulunamadı.";
                opt.PropertyName = nameof(dto.Items);
                opt.AttemptedValue = dto.Items;
            });

            return response;
        }

        decimal totalAmount = 0;

        var order = new Order
        {
            CustomerId = dto.CustomerId,
            OrderDate = DateTime.UtcNow,
            OrderItems = new List<OrderItem>()
        };

        foreach (var item in dto.Items)
        {
            var product = products.First(x => x.Id == item.ProductId);


            if (product.Stock < item.Quantity)
            {
                AddError(response, opt =>
                {
                    opt.ErrorCode = ErrorCodeValues.InsufficientStock;
                    opt.ErrorMessage = $"{product.Name} için yeterli stok yok.";
                    opt.PropertyName = nameof(item.Quantity);
                    opt.AttemptedValue = item.Quantity;
                });

                return response;
            }


            var lineTotal = product.Price * item.Quantity;
            totalAmount += lineTotal;

            product.Stock -= item.Quantity;

            order.OrderItems.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        order.TotalAmount = totalAmount;

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();

        var createdOrder = await _orderRepository.GetByIdWithDetailsAsync(order.Id);

        response.Data = _mapper.Map<OrderDto>(createdOrder);

        return response;
    }
}