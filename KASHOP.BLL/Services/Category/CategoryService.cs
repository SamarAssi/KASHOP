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

            return new Result<List<CategoryResponse>>
            {
                Success = true,
                Message = "Success",
                Data = categories.Adapt<List<CategoryResponse>>()
            };
        }
        catch (Exception exception)
        {
            return new Result<List<CategoryResponse>>
            {
                Success = false,
                Message = exception.InnerException!.Message,
            };
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
                    nameof(Category.Translations)
                }
            );

            if (category is null)
            {
                return new Result<CategoryResponse>
                {
                    Success = false,
                    Message = "Category Not Fount"
                };
            }

            return new Result<CategoryResponse>
            {
                Success = true,
                Message = "Success",
                Data = category.Adapt<CategoryResponse>()
            };
        }
        catch (Exception exception)
        {
            return new Result<CategoryResponse>
            {
                Success = false,
                Message = exception.InnerException!.Message
            };
        }
    }

    public async Task<Result<CategoryResponse>> CreateCategory(CategoryRequest request)
    {
        try
        {
            // var user = await _userManager.GetUserAsync(
            //     _httpContextAccessor.HttpContext?.User
            // );

            // if (user is null)
            // {
            //     return new Result<CategoryResponse>
            //     {
            //         Success = false,
            //         Message = "The authenticated user does not exist. Please log in again."
            //     };
            // }

            var category = request.Adapt<Category>();
            //category.CreatedById = user.Id;

            await _categoryRepository.CreateAsync(category);

            return new Result<CategoryResponse>
            {
                Success = true,
                Message = "Success",
                Data = category.Adapt<CategoryResponse>()
            };
        }
        catch (Exception exception)
        {
            return new Result<CategoryResponse>
            {
                Success = false,
                Message = exception.InnerException!.Message
            };
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
                return new Result<bool>
                {
                    Success = false,
                    Message = "Category Not Found",
                    Data = false
                };
            }

            category.Translations = new List<CategoryTranslation>();

            foreach (var translationRequest in request.Translations)
            {
                var translation = translationRequest.Adapt<CategoryTranslation>();

                category.Translations.Add(translation);
            }

            var updated = await _categoryRepository.UpdateAsync(category);

            return new Result<bool>
            {
                Success = updated,
                Message = updated ? "Success" : "Failed to Update Category",
                Data = updated
            };
        } catch(Exception exception)
        {
            return new Result<bool>
            {
                Success = false,
                Message = exception.InnerException!.Message,
                Data = false
            };
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
                return new Result<bool>
                {
                    Success = false,
                    Message = "Category Not Found",
                    Data = false
                };
            }

            var deleted = await _categoryRepository.DeleteAsync(category);

            return new Result<bool>
            {
                Success = deleted,
                Message = deleted ? "Success" : "Failed to Delete Category",
                Data = deleted
            };
        }
        catch (Exception exception)
        {
            return new Result<bool>
            {
                Success = false,
                Message = exception.InnerException!.Message,
                Data = false
            };
        }
    }
}
