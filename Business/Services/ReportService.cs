using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesManagementAPI.Core.DTOs;
using SalesManagementAPI.Core.DTOs.Orders;
using SalesManagementAPI.Core.Interfaces.Repositories;
using SalesManagementAPI.Core.Interfaces.Services;
using SalesManagementAPI.Core.Responses;

namespace SalesManagementAPI.Business.Services;

public class ReportService : BaseManager, IReportService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public ReportService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<DataResponse<SalesReportDto>> GetSalesReportAsync()
    {
        var response = new DataResponse<SalesReportDto>();

        var orders = await _orderRepository.GetAllWithDetailsAsync();

        var totalSalesAmount = orders.Sum(x => x.TotalAmount);

        var topCustomer = orders
            .GroupBy(x => new { x.CustomerId, x.Customer?.Name })
            .Select(g => new
            {
                CustomerId = g.Key.CustomerId,
                CustomerName = g.Key.Name,
                OrderCount = g.Count(),
                TotalSpent = g.Sum(x => x.TotalAmount)
            })
            .OrderByDescending(x => x.OrderCount)
            .ThenByDescending(x => x.TotalSpent)
            .FirstOrDefault();

        var topProduct = orders
            .SelectMany(x => x.OrderItems)
            .GroupBy(x => new { x.ProductId, x.Product?.Name })
            .Select(g => new
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name,
                TotalQuantity = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .FirstOrDefault();

        response.Data = new SalesReportDto
        {
            TotalSalesAmount = totalSalesAmount,
            TopCustomerName = topCustomer?.CustomerName ?? "-",
            TopCustomerOrderCount = topCustomer?.OrderCount ?? 0,
            TopProductName = topProduct?.ProductName ?? "-",
            TopProductQuantity = topProduct?.TotalQuantity ?? 0
        };

        return response;
    }

    public async Task<DataResponse<List<OrderDto>>> FilterOrdersAsync(
     DateTime? startDate,
     DateTime? endDate,
     decimal? minAmount,
     decimal? maxAmount,
     int? customerId)
    {
        var response = new DataResponse<List<OrderDto>>();

        var query = _orderRepository.Query();

        if (startDate.HasValue)
        {
            var start = startDate.Value.ToUniversalTime();
            query = query.Where(x => x.OrderDate >= start);
        }

        if (endDate.HasValue)
        {
            var end = endDate.Value.ToUniversalTime();
            query = query.Where(x => x.OrderDate <= end);
        }

        if (minAmount.HasValue)
        {
            query = query.Where(x => x.TotalAmount >= minAmount.Value);
        }

        if (maxAmount.HasValue)
        {
            query = query.Where(x => x.TotalAmount <= maxAmount.Value);
        }

        if (customerId.HasValue)
        {
            query = query.Where(x => x.CustomerId == customerId.Value);
        }

        var orders = await query.ToListAsync();
        response.Data = _mapper.Map<List<OrderDto>>(orders);

        return response;
    }


}