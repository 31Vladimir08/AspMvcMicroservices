using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsUiAsync();
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<ProductDto> GetProductAsync(int id);
        Task<IEnumerable<SupplierDto>> GetSuppliersAsync();
        Task<IEnumerable<СategoryDto>> GetСategoriesAsync();
        Task CreateProductAsync(ProductDto product);
        Task EditProductAsync(ProductDto product);
        Task DeleteProductAsync(ProductDto product);
    }
}
