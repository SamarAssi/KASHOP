using KASHOP.DAL;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace KASHOP.BLL;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(
        IUnitOfWork unitOfWork
    )
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<CategoryResponse>>> GetAllCategories()
    {
        var categories = await _unitOfWork.CategoryRepository.GetAllAsync(
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

    public async Task<Result<CategoryResponse>> GetCategory(
        Expression<Func<Category, bool>> filter
    )
    {
        var category = await _unitOfWork.CategoryRepository.GetOneAsync(
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

    public async Task<Result<CategoryResponse>> CreateCategory(CategoryRequest request)
    {
        var category = request.Adapt<Category>();

        await _unitOfWork.CategoryRepository.CreateAsync(category);
        await _unitOfWork.CompleteAsync();

        return Result<CategoryResponse>.Ok();
    }

    public async Task<Result<bool>> UpdateCategory(int id, CategoryRequest request)
    {
        var category = await _unitOfWork.CategoryRepository.GetOneAsync(
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

        _unitOfWork.CategoryRepository.UpdateAsync(category);
        var affectedRows = await _unitOfWork.CompleteAsync();

        return affectedRows > 0 ?
            Result<bool>.Ok() :
            Result<bool>.Fail("Failed to Update Category");
    }

    public async Task<Result<bool>> DeleteCategory(int id)
    {
        var category = await _unitOfWork.CategoryRepository.GetOneAsync(
            category => category.Id == id
        );

        if (category is null)
        {
            return Result<bool>.Fail("Category Not Found");
        }

        _unitOfWork.CategoryRepository.DeleteAsync(category);
        var affectedRows = await _unitOfWork.CompleteAsync();

        return affectedRows > 0 ?
            Result<bool>.Ok() :
            Result<bool>.Fail("Failed to Delete Category");
    }
}
