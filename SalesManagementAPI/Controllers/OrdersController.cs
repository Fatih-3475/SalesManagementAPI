using Microsoft.AspNetCore.Mvc;
using SalesManagementAPI.Core.DTOs.Orders;
using SalesManagementAPI.Core.Interfaces.Services;

namespace SalesManagementAPI.WebAPI.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IReportService _reportService;

    public OrdersController(IOrderService orderService, IReportService reportService)
    {
        _orderService = orderService;
        _reportService = reportService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
    {
        var result = await _orderService.CreateOrderAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] decimal? minAmount,
        [FromQuery] decimal? maxAmount,
        [FromQuery] int? customerId)
    {
        var result = await _reportService.FilterOrdersAsync(
            startDate,
            endDate,
            minAmount,
            maxAmount,
            customerId);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}