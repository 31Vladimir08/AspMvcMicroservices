using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApplication.Filters;
using WebApplication.Interfaces;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductApiController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;


        public ProductApiController(ILogger<ProductController> logger, IMapper mapper, IProductService productService)
        {
            _logger = logger;
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet]
        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var result = await _productService.GetProductsAsync();
                if (result == null || !result.Any())
                    return NotFound();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok();
                }

                await _productService.CreateProductAsync(product);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            
        }

        [HttpGet("{id:long}")]
        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public async Task<IActionResult> GetProduct(long? id)
        {
            try
            {
                if (id is null or 0)
                {
                    return NotFound();
                }

                var prodDto = await _productService.GetProductAsync((int)id);

                return Ok(prodDto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }

        [HttpPut]
        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public async Task<IActionResult> EditProduct([FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return NotFound();
                }

                await _productService.EditProductAsync(product);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            
        }

        [HttpDelete]
        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public async Task<IActionResult> DeleteProduct([FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return NotFound();
                }

                await _productService.DeleteProductAsync(product);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }
    }
}
