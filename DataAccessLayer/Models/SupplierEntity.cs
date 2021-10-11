namespace DataAccessLayer.Models
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SupplierEntity
    {
        public int SupplierID { get; set; }

        public string CompanyName { get; set; }

        public string ContactName { get; set; }

        public string ContactTitle {  get; set; }

        public string Address {  get; set; }

        public string City {  get; set; }

        public string Region {  get; set; }

        public string PostalCode {  get; set; }

        public string Country {  get; set; }

        public string Phone {  get; set; }

        public string Fax {  get; set; }

        public string HomePage {  get; set; }

        public List<ProductEntity> Products { get; set; }
    }

    public class SupplierEntityConfig : IEntityTypeConfiguration<SupplierEntity>
    {
        public void Configure(EntityTypeBuilder<SupplierEntity> builder)
        {
            builder.ToTable("Suppliers");
            builder.HasKey(u => u.SupplierID);
        }
    }
}
