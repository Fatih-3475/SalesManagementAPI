using Microsoft.AspNetCore.Mvc;
using SalesManagementAPI.Core.DTOs.Customers;
using SalesManagementAPI.Core.Interfaces.Services;

namespace SalesManagementAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService) 
        { 
        _customerService = customerService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var result = await _customerService.GetAllAsync();
            if(!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCustomerDto dto) 
        { 
            var result = await _customerService.AddAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }


    }
}
