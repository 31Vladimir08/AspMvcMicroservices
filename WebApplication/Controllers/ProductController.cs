using AutoMapper;

using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApplication.Models;
using WebApplication.ModelsUI;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        private readonly IAplicationDbContext _dbContext;
        private readonly DbSettings _options;


        public ProductController(ILogger<ProductController> logger, IOptions<DbSettings> options, IMapper mapper, IAplicationDbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
            _options = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var query = _dbContext.Set<ProductEntity>()
                    .Join(_dbContext.Set<SupplierEntity>(),
                    p => p.SupplierID,
                    c => c.SupplierID,
                    (p, c) => new ProductUI
                    {
                        ProductID = p.ProductID,
                        CategoryID = p.CategoryID,
                        ProductName = p.ProductName,
                        SupplierID = p.SupplierID,
                        SupplierName = c.CompanyName,
                        QuantityPerUnit = p.QuantityPerUnit,
                        UnitPrice = p.UnitPrice,
                        UnitsInStock = p.UnitsInStock,
                        UnitsOnOrder = p.UnitsOnOrder,
                        ReorderLevel = p.ReorderLevel,
                        Discontinued = p.Discontinued
                    })
                    .Join(_dbContext.Set<СategoryEntity>(),
                    p => p.CategoryID,
                    c => c.CategoryID,
                    (p, c) => new ProductUI
                    {
                        ProductID = p.ProductID,
                        CategoryID = p.CategoryID,
                        CategoryName = c.CategoryName,
                        ProductName = p.ProductName,
                        SupplierID = p.SupplierID,
                        SupplierName = p.SupplierName,
                        QuantityPerUnit = p.QuantityPerUnit,
                        UnitPrice = p.UnitPrice,
                        UnitsInStock = p.UnitsInStock,
                        UnitsOnOrder = p.UnitsOnOrder,
                        ReorderLevel = p.ReorderLevel,
                        Discontinued = p.Discontinued
                    });

            if (_options.MaxCountElements > 0)
            {
                query = query.Take(_options.MaxCountElements);
            }

            var result = await query.AsNoTracking().ToListAsync();
            
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var prodVM = new ProductViewModel();
            var catDb = await _dbContext.Set<СategoryEntity>().AsNoTracking().ToListAsync();
            var supDb = await _dbContext.Set<SupplierEntity>().AsNoTracking().ToListAsync();
            prodVM.Categories = _mapper.Map<IEnumerable<Сategory>>(catDb);
            prodVM.Suppliers = _mapper.Map<IEnumerable<Supplier>>(supDb);
            return View(prodVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductViewModel productUI)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(GetProducts));
            }

            var dto = _mapper.Map<Product>(productUI.Product);
            var db = _mapper.Map<ProductEntity>(dto);
            try
            {
                await _dbContext.Database.BeginTransactionAsync();
                _dbContext.Set<ProductEntity>().Add(db);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }
            return RedirectToAction(nameof(GetProducts));
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(long? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var prodVM = new ProductViewModel();
            var prodDb = await _dbContext.Set<ProductEntity>().FirstOrDefaultAsync(x => x.ProductID == id);
            var catDb = await _dbContext.Set<СategoryEntity>().AsNoTracking().ToListAsync();
            var supDb = await _dbContext.Set<SupplierEntity>().AsNoTracking().ToListAsync();
            var prodDto = _mapper.Map<Product>(prodDb);
            prodVM.Product = _mapper.Map<ProductUI>(prodDto);
            prodVM.Categories = _mapper.Map<IEnumerable<Сategory>>(catDb);
            prodVM.Suppliers = _mapper.Map<IEnumerable<Supplier>>(supDb);


            return View(prodVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductViewModel productUI)
        {
            if (!ModelState.IsValid)
            {
                var vm = productUI;
                var catDb = await _dbContext.Set<СategoryEntity>().AsNoTracking().ToListAsync();
                var supDb = await _dbContext.Set<SupplierEntity>().AsNoTracking().ToListAsync();
                vm.Categories = _mapper.Map<IEnumerable<Сategory>>(catDb);
                vm.Suppliers = _mapper.Map<IEnumerable<Supplier>>(supDb);
                return View(vm);
            }

            var dto = _mapper.Map<Product>(productUI.Product);
            var db = _mapper.Map<ProductEntity>(dto);
            try
            {
                await _dbContext.Database.BeginTransactionAsync();
                _dbContext.Set<ProductEntity>().Update(db);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                throw;
            }
            return RedirectToAction(nameof(GetProducts));
        }
    }
}
