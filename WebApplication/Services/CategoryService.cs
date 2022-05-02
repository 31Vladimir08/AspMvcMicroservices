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

namespace WebApplication.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<AplicationDbContext> _contextFactory;
        private readonly DbSettings _options;

        public CategoryService(IOptions<DbSettings> options, IMapper mapper, IDbContextFactory<AplicationDbContext> contextFactory)
        {
            _mapper = mapper;
            _contextFactory = contextFactory;
            _options = options.Value;
        }

        public async Task DeleteСategoryAsync(Сategory product)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    await context.Database.BeginTransactionAsync();
                    var category = _mapper.Map<СategoryEntity>(product);
                    context.Set<СategoryEntity>().Remove(category);

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

        public async Task EditСategoryAsync(Сategory product)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    await context.Database.BeginTransactionAsync();
                    var category = _mapper.Map<СategoryEntity>(product);
                    context.Set<СategoryEntity>().Update(category);

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

        public async Task<IEnumerable<Сategory>> GetCategoriesAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var db = await context.Set<СategoryEntity>().AsNoTracking().ToListAsync();
                var result = _mapper.Map<IEnumerable<Сategory>>(db);
                return result;
            }           
        }

        public async Task<Сategory> GetСategoryAllAsync(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var res = await context.Set<СategoryEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoryID == id);
                return _mapper.Map<Сategory>(res);
            }            
        }

        public async Task<Сategory> GetСategoryAsync(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var res = await context.Set<СategoryEntity>()
                .AsNoTracking()
                .Where(x => x.CategoryID == id)
                .Select(x => new СategoryEntity
                {
                    CategoryID = x.CategoryID,
                    CategoryName = x.CategoryName,
                    Description = x.Description,
                    Products = x.Products
                })
                .FirstOrDefaultAsync();
                return _mapper.Map<Сategory>(res);
            }  
        }

        public async Task<byte[]> GetСategoryImageAsync(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var res = await context.Set<СategoryEntity>()
                .AsNoTracking()
                .Where(x => x.CategoryID == id)
                .Select(x => x.Picture)
                .FirstOrDefaultAsync();
                return res;
            }
        }
    }
}
