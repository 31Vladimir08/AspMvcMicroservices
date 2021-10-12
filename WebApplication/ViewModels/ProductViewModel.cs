namespace WebApplication.ViewModels
{
    using System.Collections.Generic;

    using WebApplication.Models;

    public class ProductViewModel
    {
        public ProductViewModel ()
        {
            Product =  new ProductUI();
            Categories =  new List<Сategory>();
            Suppliers = new List<Supplier>();
        }

        public ProductUI Product { get; set; }

        public IEnumerable<Сategory> Categories { get; set; }

        public IEnumerable<Supplier> Suppliers { get; set; }
    }
}
