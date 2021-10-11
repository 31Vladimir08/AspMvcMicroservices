namespace DataAccessLayer.Models
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using System.ComponentModel.DataAnnotations.Schema;

    public class ProductEntity
    {
        public int ProductID { get; set; }

        public int CategoryID { get; set; }

        public string ProductName { get; set; }

        public int SupplierID { get; set; }

        public string QuantityPerUnit { get; set; }

        public decimal UnitPrice { get; set; }

        public short UnitsInStock { get; set; }

        public short UnitsOnOrder { get; set; }

        public short ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public СategoryEntity Сategory { get; set; }

        public SupplierEntity Supplier { get; set; }
    }

    public class ProductEntityConfig : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("Products");
            builder.HasOne(p => p.Сategory).WithMany(t => t.Products).HasForeignKey(p => p.CategoryID);
            builder.HasOne(p => p.Supplier).WithMany(t => t.Products).HasForeignKey(p => p.SupplierID);
            builder.HasKey(u => u.ProductID);
        }
    }
}
