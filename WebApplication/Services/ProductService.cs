using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication.Interfaces;
using WebApplication.Models;
using WebApplication.ModelsUI;

namespace WebApplication.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IAplicationDbContext _dbContext;
        private readonly DbSettings _options;

        public ProductService(IOptions<DbSettings> options, IMapper mapper, IAplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _options = options.Value;
        }

        public async Task CreateProductAsync(Product product)
        {
            await Task.Run(
                () =>
                {
                    var db = _mapper.Map<ProductEntity>(product);
                    try
                    {
                        _dbContext.Database.BeginTransaction();
                        _dbContext.Set<ProductEntity>().Add(db);
                        _dbContext.SaveChanges();
                        _dbContext.Database.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        _dbContext.Database.RollbackTransaction();
                        throw;
                    }
                });
        }

        public async Task EditProductAsync(Product product)
        {
            await Task.Run(
                () =>
                {
                    var db = _mapper.Map<ProductEntity>(product);
                    try
                    {
                        _dbContext.Database.BeginTransaction();
                        _dbContext.Set<ProductEntity>().Update(db);
                        _dbContext.SaveChanges();
                        _dbContext.Database.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        _dbContext.Database.RollbackTransaction();
                        throw;
                    }
                });
        }

        public async Task<Product> GetProductAsync(int id)
        {
            var res = await Task.Run(() =>
            {
                var result = _dbContext.Set<ProductEntity>().FirstOrDefault(x => x.ProductID == id);
                return _mapper.Map<Product>(result);
            });
            return res;
        }

        public async Task<IEnumerable<ProductUI>> GetProductsAsync()
        {
            var res = await Task.Run(
                () =>
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

                    var result = query.AsNoTracking().ToList();
                    return result;
                });
            return res;
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersAsync()
        {
            var res = await Task.Run(() =>
            {
                var result = _dbContext.Set<SupplierEntity>().AsNoTracking().ToList();
                return _mapper.Map<IEnumerable<Supplier>>(result);
            });
            return res;
        }

        public async Task<IEnumerable<Сategory>> GetСategoriesAsync()
        {
            var res = await Task.Run(() =>
            {
                var result = _dbContext.Set<СategoryEntity>().AsNoTracking().ToList();
                return _mapper.Map<IEnumerable<Сategory>>(result);
            });
            return res;
        }
    }
}
