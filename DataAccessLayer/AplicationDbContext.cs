using DataAccessLayer.Models;

using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductEntity> Product { get; set; }
        public DbSet<СategoryEntity> Сategory { get; set; }
        public DbSet<SupplierEntity> Supplier { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductEntityConfig());
            modelBuilder.ApplyConfiguration(new СategoryEntityConfig());
            modelBuilder.ApplyConfiguration(new SupplierEntityConfig());
        }
    }
}
