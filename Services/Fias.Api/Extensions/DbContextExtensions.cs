using Fias.Api.Contexts;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fias.Api.Extensions
{
    public static class DbContextExtensions
    {
        private const string SQLITE_PROVIDER_NAME = "Microsoft.EntityFrameworkCore.Sqlite";
        private const string MSSQL_PROVIDER_NAME = "Microsoft.EntityFrameworkCore.SqlServer";

        public static async Task SetIdentityInsertAsync<TEntity>(this DbContext dbContext, bool isOne) where TEntity : class
        {
            if (dbContext.Database.ProviderName != MSSQL_PROVIDER_NAME)
                return;
            var met = dbContext.Model.FindEntityType(typeof(TEntity));
            var tableName = met?.GetTableName();
            var schema = met?.GetSchema() ?? "dbo";
            if (!string.IsNullOrEmpty(tableName))
            {
                var onOff = isOne ? "ON" : "OFF";
                var query = $"SET IDENTITY_INSERT [{schema}].[{tableName}] {onOff};";
                await dbContext.Database.ExecuteSqlRawAsync(query);
            }
        }

        public static async Task DeleteFromTableWithExpressionAsync<TEntity>(this DbContext dbContext, string? queryExpression = null) where TEntity : class
        {
            var table = dbContext.Database.ProviderName == SQLITE_PROVIDER_NAME
                ? dbContext.GetTableName<TEntity>()
                : dbContext.GetTableNameWithSchema<TEntity>();
            var query = string.IsNullOrWhiteSpace(queryExpression)
                ? $"DELETE FROM {table}"
                : $"DELETE FROM {table} WHERE {queryExpression}";
            await dbContext.ExecuteSqlCommandAsync(query);
        }

        public static async Task ExecuteSqlCommandAsync(this DbContext dbContext, string sql)
        {
            var command = dbContext.Database.GetDbConnection().CreateCommand();
            command.Transaction = dbContext.Database.CurrentTransaction?.GetDbTransaction();
            var timeout = dbContext.Database.GetCommandTimeout();
            if (timeout is not null)
            {
                command.CommandTimeout = (int)timeout;
            }
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = sql;
            await command.ExecuteNonQueryAsync();
        }

        public static async Task BulkInsertAsync<TEntity>(this DbContext dbContext, IEnumerable<TEntity> datas, bool isUsePrimaryKey = false) where TEntity : class
        {
            if (datas is null || !datas.Any())
            {
                return;
            }

            var entityType = dbContext.Model.FindEntityType(typeof(TEntity));
            var tableName = dbContext.Database.ProviderName == SQLITE_PROVIDER_NAME
                ? $"[{entityType?.GetTableName()}]"
                : $"[{entityType?.GetSchema() ?? "dbo"}].[{entityType?.GetTableName()}]";
            var strColumns = GetColumnsFromEntity(datas.First(), entityType, isUsePrimaryKey);
            var sbQuery = new StringBuilder();
            var iteratorValues = 0;

            var maxRows = 50;

            string? sqlQuery;
            foreach (var entity in datas)
            {
                if (iteratorValues < maxRows)
                {
                    if (iteratorValues == 0)
                    {
                        if (dbContext.Database.ProviderName != SQLITE_PROVIDER_NAME)
                            sbQuery.Append("SET NOCOUNT ON;");
                        sbQuery.Append($"INSERT INTO {tableName}");
                        sbQuery.Append($"({strColumns})");
                        sbQuery.Append($"VALUES");
                    }

                    var strValue = GetRowFromEntity(entity, entityType, isUsePrimaryKey);

                    sbQuery.Append(strValue);

                    iteratorValues++;
                }
                else
                {
                    sqlQuery = sbQuery.ToString().TrimEnd(',');
                    await dbContext.ExecuteSqlCommandAsync(sqlQuery);
                    sbQuery = new StringBuilder();
                    iteratorValues = 0;
                }
            }

            sqlQuery = sbQuery.ToString().TrimEnd(',');
            if (!string.IsNullOrWhiteSpace(sqlQuery))
            {
                await dbContext.ExecuteSqlCommandAsync(sqlQuery);
            }
        }

        private static string GetColumnsFromEntity<TEntity>(TEntity entity, IEntityType entityType, bool isUsePrimaryKey = false) where TEntity : class
        {
            var sbColumns = new StringBuilder();
            var properties = entity.GetType().GetProperties();
            foreach (var property in properties)
            {
                var column = entityType.GetProperty(property.Name);
                if (!isUsePrimaryKey && column.IsPrimaryKey())
                    continue;
                var columnName = column.GetColumnName();
                sbColumns.Append($"[{columnName}],");
            }

            return sbColumns.ToString().TrimEnd(',');
        }

        private static string GetRowFromEntity<TEntity>(TEntity entity, IEntityType entityType, bool isUsePrimaryKey = false) where TEntity : class
        {
            var properties = entity.GetType().GetProperties();
            var sbValue = new StringBuilder();
            foreach (var property in properties)
            {
                var column = entityType.GetProperty(property.Name);
                if (!isUsePrimaryKey && column.IsPrimaryKey())
                    continue;

                var value = property.GetValue(entity);
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                var valueStr = "NULL";
                if (value is null)
                {
                    valueStr = $"NULL,";
                }
                else if (value == string.Empty)
                {
                    valueStr = $"'',";
                }
                else if (propertyType.Name == typeof(DateTime).Name || propertyType.Name == typeof(string).Name)
                {
                    valueStr = $"'{value}',";
                }
                else if (propertyType.Name == typeof(bool).Name)
                {
                    var number = Convert.ToInt16(value);
                    valueStr = $"{number},";
                }
                else
                {
                    valueStr = $"{value},";
                }

                sbValue.Append(valueStr);
            }

            var strValue = sbValue.ToString().TrimEnd(',');
            return $"({strValue}),";
        }

        private static string GetTableNameWithSchema<TEntity>(this DbContext dbContext) where TEntity : class
        {
            var met = dbContext.Model.FindEntityType(typeof(TEntity));
            var tableName = met?.GetTableName();
            var schema = met?.GetSchema() ?? "dbo";
            return $"[{schema}].[{tableName}]";
        }

        private static string GetTableName<TEntity>(this DbContext dbContext) where TEntity : class
        {
            var met = dbContext.Model.FindEntityType(typeof(TEntity));
            var tableName = met?.GetTableName();
            return $"[{tableName}]";
        }
    }
}
