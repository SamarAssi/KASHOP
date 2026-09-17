using System.Linq.Expressions;
using KASHOP.DAL;
using Mapster;
using Microsoft.AspNetCore.Components.Forms;

namespace KASHOP.BLL;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public ProductService(
        IUnitOfWork unitOfWork, 
        IFileService fileService
    )
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<Result<ProductResponse>> CreateProduct(ProductRequest request)
    {
        if (request.MainImage is null)
        {
            return Result<ProductResponse>.Fail("Main image is required");
        }

        var uploadedResult = await _fileService.UploadAsync(request.MainImage);

        if (!uploadedResult.Success)
        {
            return Result<ProductResponse>.Fail(uploadedResult.Message);
        }

        var product = request.Adapt<Product>();
        product.MainImage = uploadedResult.Data!;

        await _unitOfWork.ProductRepository.CreateAsync(product);
        await _unitOfWork.CompleteAsync();

        return Result<ProductResponse>.Ok();
    }

    public async Task<Result<List<ProductResponse>>> GetAllProducts()
    {
        var products = await _unitOfWork.ProductRepository.GetAllAsync(
            new string[]
            {
                    nameof(Product.Translations),
                    nameof(Product.Category)
            }
        );

        return Result<List<ProductResponse>>.Ok(
            "Success",
            products.Adapt<List<ProductResponse>>()
        );
    }

    public async Task<Result<ProductResponse>> GetProduct(Expression<Func<Product, bool>> filter)
    {
        var product = await _unitOfWork.ProductRepository.GetOneAsync(
            filter,
            new string[]
            {
                    nameof(Product.Translations),
                    nameof(Product.Category)
            }
        );

        if (product is null)
        {
            return Result<ProductResponse>.Fail("Product Not Found");
        }

        return Result<ProductResponse>.Ok(
            "Success",
            product.Adapt<ProductResponse>()
        );
    }
}
