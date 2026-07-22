using System.Linq.Expressions;
using KASHOP.DAL;

namespace KASHOP.BLL;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetAllCategories();
    Task<CategoryResponse> GetCategory(Expression<Func<Category, bool>> filter);
    Task<CategoryResponse> CreateCategory(CategoryRequest request);
    Task<bool> UpdateCategory(int id, CategoryRequest request);
    Task<bool> DeleteCategory(int id);
}
