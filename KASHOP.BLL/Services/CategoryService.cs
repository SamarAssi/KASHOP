using KASHOP.DAL;
using Mapster;

namespace KASHOP.BLL;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryResponse>> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllAsync();

        return categories.Adapt<List<CategoryResponse>>();
    }

    public async Task<CategoryResponse> CreateCategory(CategoryRequest request)
    {
        var category = request.Adapt<Category>();
        
        await _categoryRepository.CreateAsync(category);

        return category.Adapt<CategoryResponse>();
    }
}
