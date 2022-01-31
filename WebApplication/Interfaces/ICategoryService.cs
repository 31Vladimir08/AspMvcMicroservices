using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Сategory>> GetCategoriesAsync();
        Task<Сategory> GetСategoryAsync(int id);
        Task<Сategory> GetСategoryAllAsync(int id);
        Task<byte[]> GetСategoryImageAsync(int id);
        Task EditСategoryAsync(Сategory product);
        Task DeleteСategoryAsync(Сategory product);
    }
}
