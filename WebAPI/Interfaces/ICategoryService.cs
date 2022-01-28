using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<СategoryDto>> GetCategoriesAsync();
        Task<СategoryDto> GetСategoryAsync(int id);
        Task<СategoryDto> GetСategoryAllAsync(int id);
        Task<byte[]> GetСategoryImageAsync(int id);
        Task EditСategoryAsync(СategoryDto product);
        Task DeleteСategoryAsync(СategoryDto product);
    }
}
