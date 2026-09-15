using KASHOP.DAL;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace KASHOP.BLL;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager
    )
    {
        _categoryRepository = categoryRepository;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<Result<List<CategoryResponse>>> GetAllCategories()
    {
        try
        {
            var categories = await _categoryRepository.GetAllAsync(
                new string[]
                {
                    nameof(Category.Translations),
                    nameof(Category.CreatedBy)
                }
            );

            return Result<List<CategoryResponse>>.Ok(
                "Success",
                categories.Adapt<List<CategoryResponse>>()
            );
        }
        catch (Exception exception)
        {
            return Result<List<CategoryResponse>>.Fail(exception.InnerException!.Message);
        }
    }

    public async Task<Result<CategoryResponse>> GetCategory(
        Expression<Func<Category, bool>> filter
    )
    {
        try
        {
            var category = await _categoryRepository.GetOneAsync(
                filter,
                new string[]
                {
                    nameof(Category.Translations),
                    nameof(Category.CreatedBy)
                }
            );

            if (category is null)
            {
                return Result<CategoryResponse>.Fail("Category Not Found");
            }

            return Result<CategoryResponse>.Ok(
                "Success",
                category.Adapt<CategoryResponse>()
            );
        }
        catch (Exception exception)
        {
            return Result<CategoryResponse>.Ok(exception.InnerException!.Message);
        }
    }

    public async Task<Result<CategoryResponse>> CreateCategory(CategoryRequest request)
    {
        try
        {
            var category = request.Adapt<Category>();

            await _categoryRepository.CreateAsync(category);

            return Result<CategoryResponse>.Ok();
        }
        catch (Exception exception)
        {
            return Result<CategoryResponse>.Fail(exception.InnerException!.Message);
        }
    }

    public async Task<Result<bool>> UpdateCategory(int id, CategoryRequest request)
    {
        try
        {
            var category = await _categoryRepository.GetOneAsync(
                category => category.Id == id,
                new string[]
                {
                    nameof(Category.Translations)
                }
            );

            if (category is null)
            {
                return Result<bool>.Fail("Category Not Found");
            }

            category.Translations = new List<CategoryTranslation>();

            foreach (var translationRequest in request.Translations)
            {
                var translation = translationRequest.Adapt<CategoryTranslation>();

                category.Translations.Add(translation);
            }

            var updated = await _categoryRepository.UpdateAsync(category);

            return updated ?
                Result<bool>.Ok() :
                Result<bool>.Fail("Failed to Update Category");
        } catch(Exception exception)
        {
            return Result<bool>.Fail(exception.InnerException!.Message);
        }
    }

    public async Task<Result<bool>> DeleteCategory(int id)
    {
        try
        {
            var category = await _categoryRepository.GetOneAsync(
                category => category.Id == id
            );

            if (category is null)
            {
                return Result<bool>.Fail("Category Not Found");
            }

            var deleted = await _categoryRepository.DeleteAsync(category);

            return deleted ?
                Result<bool>.Ok() :
                Result<bool>.Fail("Failed to Delete Category");
        }
        catch (Exception exception)
        {
            return Result<bool>.Fail(exception.InnerException!.Message);
        }
    }
}
