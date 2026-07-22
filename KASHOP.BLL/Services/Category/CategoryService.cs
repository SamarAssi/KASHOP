using KASHOP.DAL;
using Mapster;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

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

    public async Task<bool> UpdateCategory(int id, CategoryRequest request)
    {
        var category = await _categoryRepository.GetOneAsync(
            category => category.Id == id,
            new string[]
            {
                nameof(Category.Translations)
            }
        );

        if (category == null) return false;

        category.Translations = new List<CategoryTranslation>();

        foreach (var translationRequest in request.Translations)
        {
            var translation = translationRequest.Adapt<CategoryTranslation>();

            category.Translations.Add(translation);
        }

        return await _categoryRepository.UpdateAsync(category);
    }

    public async Task<bool> DeleteCategory(int id)
    {
        var category = await _categoryRepository.GetOneAsync(category => category.Id == id);

        return category == null ?
        false :
        await _categoryRepository.DeleteAsync(category);
    }
}
