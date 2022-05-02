using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

using DataAccessLayer;
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
        private readonly IDbContextFactory<AplicationDbContext> _contextFactory;
        private readonly DbSettings _options;

        public ProductService(IOptions<DbSettings> options, IMapper mapper, IDbContextFactory<AplicationDbContext> contextFactory)
        {
            _mapper = mapper;
            _contextFactory = contextFactory;
            _options = options.Value;
        }

        public async Task CreateProductAsync(Product product)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    await context.Database.BeginTransactionAsync();
                    var db = _mapper.Map<ProductEntity>(product);
                    context.Set<ProductEntity>().Add(db);
                    await context.SaveChangesAsync();
                    await context.Database.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    await context.Database.RollbackTransactionAsync();
                    throw;
                }
            }
        }

        public async Task DeleteProductAsync(Product product)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    await context.Database.BeginTransactionAsync();
                    var db = _mapper.Map<ProductEntity>(product);
                    context.Set<ProductEntity>().Remove(db);
                    await context.SaveChangesAsync();
                    await context.Database.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    await context.Database.RollbackTransactionAsync();
                    throw;
                }
            }
        }

        public async Task EditProductAsync(Product product)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    await context.Database.BeginTransactionAsync();
                    var db = _mapper.Map<ProductEntity>(product);
                    context.Set<ProductEntity>().Update(db);
                    await context.SaveChangesAsync();
                    await context.Database.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    await context.Database.RollbackTransactionAsync();
                    throw;
                }
            }
        }

        public async Task<Product> GetProductAsync(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var result = await context.Set<ProductEntity>().FirstOrDefaultAsync(x => x.ProductID == id);
                return _mapper.Map<Product>(result);
            }
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var query = context.Set<ProductEntity>().AsQueryable();

                if (_options.MaxCountElements > 0)
                {
                    query = query.Take(_options.MaxCountElements);
                }

                var result = await query.AsNoTracking().ToListAsync();
                var res = _mapper.Map<IEnumerable<Product>>(result);
                return res;
            }
        }

        public async Task<IEnumerable<ProductUI>> GetProductsUiAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var query = context.Set<ProductEntity>()
                        .Join(context.Set<SupplierEntity>(),
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
                        .Join(context.Set<СategoryEntity>(),
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
                return result;
            }            
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var result = await context.Set<SupplierEntity>().AsNoTracking().ToListAsync();
                return _mapper.Map<IEnumerable<Supplier>>(result);
            }
        }

        public async Task<IEnumerable<Сategory>> GetСategoriesAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var result = await context.Set<СategoryEntity>().AsNoTracking().ToListAsync();
                return _mapper.Map<IEnumerable<Сategory>>(result);
            }
        }
    }
}
