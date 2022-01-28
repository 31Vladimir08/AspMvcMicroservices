using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Services
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

        public async Task CreateProductAsync(ProductDto product)
        {
            var db = _mapper.Map<ProductEntity>(product);
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
        }

        public async Task DeleteProductAsync(ProductDto product)
        {
            var db = _mapper.Map<ProductEntity>(product);
            try
            {
                await _dbContext.Database.BeginTransactionAsync();
                _dbContext.Set<ProductEntity>().Remove(db);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task EditProductAsync(ProductDto product)
        {
            var db = _mapper.Map<ProductEntity>(product);
            try
            {
                await _dbContext.Database.BeginTransactionAsync();
                _dbContext.Set<ProductEntity>().Update(db);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<ProductDto> GetProductAsync(int id)
        {
            var result = await _dbContext.Set<ProductEntity>().FirstOrDefaultAsync(x => x.ProductID == id);
            return _mapper.Map<ProductDto>(result);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var query = _dbContext.Set<ProductEntity>().AsQueryable();

            if (_options.MaxCountElements > 0)
            {
                query = query.Take(_options.MaxCountElements);
            }

            var result = await query.AsNoTracking().ToListAsync();
            var res = _mapper.Map<IEnumerable<ProductDto>>(result);
            return res;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsUiAsync()
        {
            var query = _dbContext.Set<ProductEntity>()
                        .Join(_dbContext.Set<SupplierEntity>(),
                            p => p.SupplierID,
                            c => c.SupplierID,
                            (p, c) => new ProductDto
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
                            (p, c) => new ProductDto
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

        public async Task<IEnumerable<SupplierDto>> GetSuppliersAsync()
        {
            var result = await _dbContext.Set<SupplierEntity>().AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<SupplierDto>>(result);
        }

        public async Task<IEnumerable<СategoryDto>> GetСategoriesAsync()
        {
            var result = await _dbContext.Set<СategoryEntity>().AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<СategoryDto>>(result);
        }
    }
}
