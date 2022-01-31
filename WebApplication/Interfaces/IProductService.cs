using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Models;
using WebApplication.ModelsUI;

namespace WebApplication.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductUI>> GetProductsUiAsync();
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(int id);
        Task<IEnumerable<Supplier>> GetSuppliersAsync();
        Task<IEnumerable<Сategory>> GetСategoriesAsync();
        Task CreateProductAsync(Product product);
        Task EditProductAsync(Product product);
        Task DeleteProductAsync(Product product);
    }
}
