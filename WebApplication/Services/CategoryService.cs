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
            await Task.Run(() =>
            {
                try
                {
                    var category = _mapper.Map<СategoryEntity>(product);
                    _dbContext.Database.BeginTransaction();
                    _dbContext.Set<СategoryEntity>().Update(category);

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

        public async Task<IEnumerable<Сategory>> GetCategoriesAsync()
        {
            var res = await Task.Run(() =>
            {
                var db = _dbContext.Set<СategoryEntity>().AsNoTracking().ToList();
                var result = _mapper.Map<IEnumerable<Сategory>>(db);
                return result;
            });
            return res;
        }

        public async Task<Сategory> GetСategoryAsync(int id)
        {
            var res = await Task.Run(() =>
            {
                var res = _dbContext.Set<СategoryEntity>()
                    .AsNoTracking()
                    .Where(x => x.CategoryID == id)
                    .Select(x => new СategoryEntity
                    {
                        CategoryID = x.CategoryID,
                        CategoryName = x.CategoryName,
                        Description = x.Description,
                        Products = x.Products
                    })
                    .FirstOrDefault();
                return _mapper.Map<Сategory>(res);
            });
            return res;
        }

        public async Task<byte[]> GetСategoryImageAsync(int id)
        {
            var result = await Task.Run(() =>
            {
                var res = _dbContext.Set<СategoryEntity>()
                    .AsNoTracking()
                    .Where(x => x.CategoryID == id)
                    .Select(x => x.Picture)
                    .FirstOrDefault();
                return res;
            });
            return result;
        }
    }
}
