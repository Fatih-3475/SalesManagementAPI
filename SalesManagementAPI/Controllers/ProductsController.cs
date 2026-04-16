using Microsoft.AspNetCore.Mvc;
using SalesManagementAPI.Core.DTOs.Products;
using SalesManagementAPI.Core.Interfaces.Services;

namespace SalesManagementAPI.WebAPI.Controllers
{ 
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
       private readonly IProductService _productService;
        public ProductsController(IProductService productService) 
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetAllAsync();

            if(!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateProductDto dto)
        {
            var result = await _productService.AddAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
