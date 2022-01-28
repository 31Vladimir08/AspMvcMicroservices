using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;


        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
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
        public async Task<IActionResult> AddProduct([FromBody] ProductDto product)
        {
            if (!ModelState.IsValid)
            {
                return Ok();
            }

            try
            {
                await _productService.CreateProductAsync(product);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetProduct(long? id)
        {
            
            if (id is null or 0)
            {
                return NotFound();
            }

            try
            {
                var prodDto = await _productService.GetProductAsync((int)id);

                return Ok(prodDto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditProduct([FromBody] ProductDto product)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            try
            {
                await _productService.EditProductAsync(product);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromBody] ProductDto product)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            try
            {
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
