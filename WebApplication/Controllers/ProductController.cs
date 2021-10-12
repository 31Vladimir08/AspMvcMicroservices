﻿namespace WebApplication.Controllers
{
    using AutoMapper;

    using DataAccessLayer.Interfaces;
    using DataAccessLayer.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using WebApplication.Models;
    using WebApplication.ViewModels;

    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        private readonly IAplicationDbContext _dbContext;
        private readonly int _maxCountElement;


        public ProductController(ILogger<ProductController> logger, IConfiguration configuration, IMapper mapper, IAplicationDbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
            _maxCountElement = string.IsNullOrWhiteSpace(configuration["DbSettings:MaxCountElements"]) ? 0 : int.Parse(configuration["DbSettings:MaxCountElements"]);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var result = await Task.Run(
                () =>
                {
                    var query = _dbContext.Set<ProductEntity>()
                    .Join(_dbContext.Set<SupplierEntity>().AsNoTracking(),
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
                    .Join(_dbContext.Set<СategoryEntity>().AsNoTracking(),
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

                    if (_maxCountElement != 0)
                    {
                        query = query.Take(_maxCountElement);
                    }

                    return query.AsNoTracking().ToList();
                });
            return View(result ?? new List<ProductUI>());
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            
            var productVM = await Task.Run(
                () =>
                {
                    var prodVM = new ProductViewModel();
                    var catDb = _dbContext.Set<СategoryEntity>().AsNoTracking().ToList();
                    var supDb = _dbContext.Set<SupplierEntity>().AsNoTracking().ToList();
                    prodVM.Categories = _mapper.Map<IEnumerable<Сategory>>(catDb);
                    prodVM.Suppliers = _mapper.Map<IEnumerable<Supplier>>(supDb);
                    return prodVM;
                });
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductViewModel personUi)
        {
            await Task.Run(
                () =>
                {
                    var dto = _mapper.Map<Product>(personUi.Product);
                    var db = _mapper.Map<ProductEntity>(dto);
                    _dbContext.Set<ProductEntity>().Add(db);
                });
            return RedirectToAction(nameof(GetProducts));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductUI personUi)
        {
            return View(personUi);
        }
    }
}
