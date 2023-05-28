using Fias.Api.Entities;
using Fias.Api.Enums;
using Fias.Api.Models.Options.DataBase;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Fias.Api.Contexts
{
    public class AppDbContext : DbContext
    {
        private readonly DbSettingsOption _dbOptions;
        public AppDbContext(DbContextOptions<AppDbContext> options, IOptions<DbSettingsOption> dbOptions)
            : base(options)
        {
            _dbOptions = dbOptions.Value;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (_dbOptions.TypeDb)
            {
                case SupportedDb.SQLite:
                    optionsBuilder.UseSqlite(_dbOptions.ConnectionString);
                    break;
                case SupportedDb.MSSQL:
                    optionsBuilder.UseSqlServer(_dbOptions.ConnectionString);
                    break;
                default:
                    throw new InvalidOperationException();
            }
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            switch (_dbOptions.TypeDb)
            {
                case SupportedDb.SQLite:
                    OnSQLiteModelCreating(modelBuilder);
                    break;
                case SupportedDb.MSSQL:
                    OnMSSQLModelCreating(modelBuilder);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void OnSQLiteModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AddrObjEntitySQLiteConfig());
            modelBuilder.ApplyConfiguration(new AddrObjParamEntitySQLiteConfig());
            modelBuilder.ApplyConfiguration(new HouseEntitySQLiteConfig());
            modelBuilder.ApplyConfiguration(new HouseParamsEntitySQLiteConfig());
            modelBuilder.ApplyConfiguration(new ParamTypesEntitySQLiteConfig());
        }

        private void OnMSSQLModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AddrObjEntityMSSQLConfig());
            modelBuilder.ApplyConfiguration(new AddrObjParamEntityMSSQLConfig());
            modelBuilder.ApplyConfiguration(new HouseEntityMSSQLConfig());
            modelBuilder.ApplyConfiguration(new HouseParamsEntityMSSQLConfig());
            modelBuilder.ApplyConfiguration(new ParamTypesEntityMSSQLConfig());
        }
    }
}
