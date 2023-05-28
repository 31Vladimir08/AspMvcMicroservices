using Fias.Api.Contexts;
using Fias.Api.Entities;
using Fias.Api.Enums;
using Fias.Api.Interfaces.Repositories;
using Fias.Api.Models.Options.DataBase;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Fias.Api.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSettingsOption _dbOptions;

        public BaseRepository(AppDbContext dbContext, IOptions<DbSettingsOption> dbOptions) 
        {
            _dbContext = dbContext;
            _dbOptions = dbOptions.Value;
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

            SetIdentityInsert<TEntity>(true);

            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            SetIdentityInsert<TEntity>(false);
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
                SetIdentityInsert<TEntity>(true);

                foreach (var entity in entities)
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
                }
                
                await _dbContext.SaveChangesAsync();

                SetIdentityInsert<TEntity>(false);
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

        private void SetIdentityInsert<TEntity>(bool isOne) where TEntity : BaseEntity
        {
            if (_dbOptions.TypeDb != SupportedDb.MSSQL)
                return;
            var met = _dbContext.Model.FindEntityType(typeof(TEntity));
            var tableName = met?.GetTableName();
            var schema = met?.GetSchema() ?? "dbo";
            if (!string.IsNullOrEmpty(tableName))
            {
                var onOff = isOne ? "ON" : "OFF";
                _dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {schema}.{tableName} {onOff};");
            }
        }
    }
}
