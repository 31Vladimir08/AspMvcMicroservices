﻿using Fias.Api.Contexts;
using Fias.Api.Entities;

namespace Fias.Api.Interfaces.Repositories
{
    public interface IBaseRepository
    {
        Task UpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
        Task InsertsAsync<TEntity>(List<TEntity>? entities) where TEntity : BaseEntity;
        Task InsertsOrUpdatesAsync<TEntity>(List<TEntity>? entities) where TEntity : BaseEntity;
        Task DeleteAllEntitiesAsync<TEntity>() where TEntity : BaseEntity;
        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
        Task InsertOrUpdateAsync<TEntity>(TEntity? entity) where TEntity : BaseEntity;
        Task InsertAsync<TEntity>(TEntity? entities) where TEntity : BaseEntity;
        void SetIdentityInsert<TEntity>(bool isOne) where TEntity : BaseEntity;
    }
}
