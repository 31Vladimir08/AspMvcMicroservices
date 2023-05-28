using Fias.Api.Contexts;
using Fias.Api.Entities;
using Fias.Api.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Fias.Api.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly AppDbContext _dbContext;

        public BaseRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task DeleteAllEntitiesAsync<TEntity>() where TEntity : BaseEntity
        {
            _dbContext.Set<TEntity>().RemoveRange(_dbContext.Set<TEntity>());
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task InsertsAsync<TEntity>(List<TEntity>? entities) where TEntity : BaseEntity
        {
            if (entities is null || entities.Count == 0)
            {
                return;
            }

            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task InsertsOrUpdatesAsync<TEntity>(List<TEntity>? entities) where TEntity : BaseEntity
        {
            if (entities is null || entities.Count == 0)
            {
                return;
            }

            try
            {
                _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

                entities.ForEach(async entity =>
                {
                    if (await _dbContext.Set<TEntity>()
                    .AsNoTracking().AnyAsync(q => q.Id == entity.Id))
                    {
                        _dbContext.Set<TEntity>().Update(entity);
                    }
                    else
                    {
                        _dbContext.Set<TEntity>().Add(entity);
                    }
                });

                await _dbContext.SaveChangesAsync();
            }
            finally
            {
                _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
