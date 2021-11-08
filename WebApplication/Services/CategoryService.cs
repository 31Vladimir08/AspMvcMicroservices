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

namespace WebApplication.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IAplicationDbContext _dbContext;
        private readonly DbSettings _options;

        public CategoryService(IOptions<DbSettings> options, IMapper mapper, IAplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _options = options.Value;
        }

        public async Task EditСategoryAsync(Сategory product)
        {
            try
            {
                var category = _mapper.Map<СategoryEntity>(product);
                await _dbContext.Database.BeginTransactionAsync();
                _dbContext.Set<СategoryEntity>().Update(category);

                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Сategory>> GetCategoriesAsync()
        {
            var db = await _dbContext.Set<СategoryEntity>().AsNoTracking().ToListAsync();
            var result = _mapper.Map<IEnumerable<Сategory>>(db);
            return result;
        }

        public async Task<Сategory> GetСategoryAsync(int id)
        {
            var res = await _dbContext.Set<СategoryEntity>()
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

        public async Task<byte[]> GetСategoryImageAsync(int id)
        {
            var res = await _dbContext.Set<СategoryEntity>()
                .AsNoTracking()
                .Where(x => x.CategoryID == id)
                .Select(x => x.Picture)
                .FirstOrDefaultAsync();
            return res;
        }
    }
}
