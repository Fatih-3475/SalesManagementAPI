using Microsoft.AspNetCore.Mvc;
using SalesManagementAPI.Core.Interfaces.Services;

namespace SalesManagementAPI.WebAPI.Controllers;

[ApiController]
[Route("reports")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("sales")]
    public async Task<IActionResult> GetSalesReport()
    {
        var result = await _reportService.GetSalesReportAsync();

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}