using System.Collections.Generic;

using Fias.Api.Contexts;
using Fias.Api.Entities;
using Fias.Api.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Fias.Api.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        public BaseRepository() 
        {
        }

        public async Task DeleteAllEntitiesAsync<TEntity>(AppDbContext dbContext) where TEntity : BaseEntity
        {
            dbContext.Set<TEntity>().RemoveRange(dbContext.Set<TEntity>());
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync<TEntity>(TEntity entity, AppDbContext dbContext) where TEntity : BaseEntity
        {
            dbContext.Set<TEntity>().Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task InsertsAsync<TEntity>(List<TEntity>? entities, AppDbContext dbContext) where TEntity : BaseEntity
        {
            if (entities is null || entities.Count == 0)
            {
                return;
            }

            var tableName = dbContext.Model.FindEntityType(typeof(TEntity))?.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
            {
                dbContext.Set<TEntity>().FromSqlRaw($"SET IDENTITY_INSERT dbo.{tableName} ON;");
            }
            await dbContext.Set<TEntity>().AddRangeAsync(entities);
            await dbContext.SaveChangesAsync();

            if (!string.IsNullOrEmpty(tableName))
            {
                dbContext.Set<TEntity>().FromSqlRaw($"SET IDENTITY_INSERT dbo.{tableName} OFF;");
            }
        }

        public async Task InsertsOrUpdatesAsync<TEntity>(List<TEntity>? entities, AppDbContext dbContext) where TEntity : BaseEntity
        {
            if (entities is null || entities.Count == 0)
            {
                return;
            }

            try
            {
                dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                var tableName = dbContext.Model.FindEntityType(typeof(TEntity))?.GetTableName();
                if (!string.IsNullOrEmpty(tableName))
                {
                    dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT dbo.{tableName} ON;");
                }

                foreach ( var entity in entities )
                {
                    if (await dbContext.Set<TEntity>()
                    .AsNoTracking().AnyAsync(q => q.Id == entity.Id))
                    {
                        dbContext.Set<TEntity>().Update(entity);
                    }
                    else
                    {
                        dbContext.Set<TEntity>().Add(entity);
                    }
                }
                
                await dbContext.SaveChangesAsync();
                if (!string.IsNullOrEmpty(tableName))
                {
                    dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT dbo.{tableName} OFF;");
                }
            }
            finally
            {
                dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        public async Task UpdateAsync<TEntity>(TEntity entity, AppDbContext dbContext) where TEntity : BaseEntity
        {
            dbContext.Set<TEntity>().Update(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
