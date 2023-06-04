using Microsoft.EntityFrameworkCore;

namespace Fias.Api.Extensions
{
    public static class DbContextExtensions
    {
        public static void SetIdentityInsert<TEntity>(this DbContext dbContext, bool isOne) where TEntity : class
        {
            var met = dbContext.Model.FindEntityType(typeof(TEntity));
            var tableName = met?.GetTableName();
            var schema = met?.GetSchema() ?? "dbo";
            if (!string.IsNullOrEmpty(tableName))
            {
                var onOff = isOne ? "ON" : "OFF";
                dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [{schema}].[{tableName}] {onOff};");
            }
        }
    }
}
