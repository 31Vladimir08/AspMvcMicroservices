namespace WebApplication.ModelsUI
{
    using System.ComponentModel.DataAnnotations;

    public class ProductUI
    {
        public int ProductID { get; set; }

        [Required (ErrorMessage = "Product name not specified")]
        [StringLength(40)]
        public string ProductName { get; set; }

        public int? SupplierID { get; set; }

        public string SupplierName { get; set; }

        public int? CategoryID { get; set; }

        public string CategoryName { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        [Required(ErrorMessage = "Discontinued not specified")]
        public bool Discontinued { get; set; }
    }
}
