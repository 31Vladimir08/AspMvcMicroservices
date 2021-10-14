namespace WebApplication.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ProductUI
    {
        public int ProductID { get; set; }

        [Required (ErrorMessage = "Product name not specified")]
        [StringLength(40, ErrorMessage = "String length must be up to 40 characters")]
        public string ProductName { get; set; }

        public int? SupplierID { get; set; }

        public string SupplierName { get; set; }

        public int? CategoryID { get; set; }

        public string CategoryName { get; set; }

        [StringLength(20, ErrorMessage = "String length must be up to 20 characters")]
        public string QuantityPerUnit { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        [Required(ErrorMessage = "Discontinued not specified")]
        public bool Discontinued { get; set; }
    }
}
