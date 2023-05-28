using Fias.Api.Contexts;
using Fias.Api.Entities;

namespace Fias.Api.Interfaces.Repositories
{
    public interface IBaseRepository
    {
        Task UpdateAsync<TEntity>(TEntity entity, AppDbContext dbContext) where TEntity : BaseEntity;
        Task InsertsAsync<TEntity>(List<TEntity>? entities, AppDbContext dbContext) where TEntity : BaseEntity;
        Task InsertsOrUpdatesAsync<TEntity>(List<TEntity>? entities, AppDbContext dbContext) where TEntity : BaseEntity;
        Task DeleteAllEntitiesAsync<TEntity>(AppDbContext dbContext) where TEntity : BaseEntity;
        Task DeleteAsync<TEntity>(TEntity entity, AppDbContext dbContext) where TEntity : BaseEntity;
    }
}
