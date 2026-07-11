using KASHOP.DAL;
using Mapster;
using System.Linq.Expressions;

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
        var categories = await _categoryRepository.GetAllAsync(
            new string[]
            {
                nameof(Category.Translations)
            }
        );

        return categories.Adapt<List<CategoryResponse>>();
    }

    public async Task<CategoryResponse> GetCategory(Expression<Func<Category, bool>> filter)
    {
        var category = await _categoryRepository.GetOneAsync(
            filter,
            new string[]
            {
                nameof(Category.Translations)
            }
        );

        return category.Adapt<CategoryResponse>();
    }

    public async Task<CategoryResponse> CreateCategory(CategoryRequest request)
    {
        var category = request.Adapt<Category>();
        
        await _categoryRepository.CreateAsync(category);

        return category.Adapt<CategoryResponse>();
    }
}
