using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class СategoryEntity
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public List<ProductEntity> Products { get; set; }
    }

    public class СategoryEntityConfig : IEntityTypeConfiguration<СategoryEntity>
    {
        public void Configure(EntityTypeBuilder<СategoryEntity> builder)
        {
            builder.ToTable("Categories");
            builder.Property(u => u.CategoryName).HasColumnName("CategoryName");
            builder.Property(u => u.Description).HasColumnName("Description");
            builder.HasKey(u => u.CategoryID);
        }
    }
}
