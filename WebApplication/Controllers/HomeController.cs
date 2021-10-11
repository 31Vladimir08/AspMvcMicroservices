namespace WebApplication.Controllers
{
    using AutoMapper;

    using DataAccessLayer.Interfaces;
    using DataAccessLayer.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using WebApplication.Models;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IAplicationDbContext _dbContext;


        public HomeController(ILogger<HomeController> logger, IMapper mapper, IAplicationDbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Product()
        {
            var result = await Task.Run(
                () =>
                {
                    var res = _dbContext.Set<ProductEntity>()
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
                    .AsNoTracking()
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
                    })
                    .AsNoTracking()
                    .ToList();

                    return res;
                });
            return View(result ?? new List<ProductUI>());
        }

        public async Task<IActionResult> Category()
        {
            var result = await Task.Run(
                () =>
                {
                    var db = _dbContext.Set<СategoryEntity>().AsNoTracking().ToList();
                    var res = _mapper.Map<IEnumerable<Сategory>>(db);
                    return res; 
                });
            return View(result ?? new List<Сategory>());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
