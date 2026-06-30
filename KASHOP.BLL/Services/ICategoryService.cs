using KASHOP.DAL;

namespace KASHOP.BLL;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetAllCategories();
    Task<CategoryResponse> CreateCategory(CategoryRequest request);
}
