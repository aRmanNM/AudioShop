using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(Category category);
        Category UpdateCategory(Category category);
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> DeleteCategory(int categoryId);
    }
}