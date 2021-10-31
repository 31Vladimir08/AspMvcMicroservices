using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApplication.Filters;
using WebApplication.Interfaces;
using WebApplication.Models;
using WebApplication.ModelsUI;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    [ServiceFilter(typeof(LogingCallsActionFilter))]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;


        public ProductController(ILogger<ProductController> logger, IMapper mapper, IProductService productService)
        {
            _logger = logger;
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet]
        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _productService.GetProductsAsync();

            return View(result);
        }

        [HttpGet]
        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public async Task<IActionResult> CreateProduct()
        {
            var prodVM = new ProductViewModel();
            prodVM.Categories = await _productService.GetСategoriesAsync();
            prodVM.Suppliers = await _productService.GetSuppliersAsync();
            return View(prodVM);
        }

        [HttpPost]
        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public async Task<IActionResult> CreateProduct(ProductViewModel productUI)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(GetProducts));
            }

            var dto = _mapper.Map<Product>(productUI.Product);

            await _productService.CreateProductAsync(dto);
           
            return RedirectToAction(nameof(GetProducts));
        }

        [HttpGet]
        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public async Task<IActionResult> EditProduct(long? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            
            var prodVM = new ProductViewModel();
            var prodDto = await _productService.GetProductAsync((int)id);
            prodVM.Product = _mapper.Map<ProductUI>(prodDto);
            prodVM.Categories = await _productService.GetСategoriesAsync();
            prodVM.Suppliers = await _productService.GetSuppliersAsync();
            
            return View(prodVM);
        }

        [HttpPost]
        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public async Task<IActionResult> EditProduct(ProductViewModel productUI)
        {
            if (!ModelState.IsValid)
            {
                var vm = productUI;
                vm.Categories = await _productService.GetСategoriesAsync();
                vm.Suppliers = await _productService.GetSuppliersAsync();
                return View(vm);
            }

            var dto = _mapper.Map<Product>(productUI.Product);
            await _productService.EditProductAsync(dto);

            return RedirectToAction(nameof(GetProducts));
        }
    }
}
