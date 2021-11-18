using System.Collections.Generic;

namespace ConsoleApp.Models
{
    public class Сategory
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public List<Product> Products { get; set; }
    }
}
