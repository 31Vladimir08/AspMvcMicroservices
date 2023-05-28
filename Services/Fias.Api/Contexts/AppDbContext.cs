using Fias.Api.Entities;
using Fias.Api.Enums;
using Fias.Api.Interfaces;
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
    }
}
