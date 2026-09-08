using System.Linq.Expressions;
using KASHOP.DAL;

namespace KASHOP.BLL;

public interface ICategoryService
{
    Task<Result<List<CategoryResponse>>> GetAllCategories();
    Task<Result<CategoryResponse>> GetCategory(Expression<Func<Category, bool>> filter);
    Task<Result<CategoryResponse>> CreateCategory(CategoryRequest request);
    Task<Result<bool>> UpdateCategory(int id, CategoryRequest request);
    Task<Result<bool>> DeleteCategory(int id);
}
