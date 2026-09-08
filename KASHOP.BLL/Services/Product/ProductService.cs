using KASHOP.DAL;
using Mapster;

namespace KASHOP.BLL;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IFileService _fileService;

    public ProductService(IProductRepository productRepository, IFileService fileService)
    {
        _productRepository = productRepository;
        _fileService = fileService;
    }

    public async Task<Result<ProductResponse>> CreateProduct(ProductRequest request)
    {
        try
        {
            if (request.MainImage is null)
            {
                return new Result<ProductResponse>
                {
                    Success = false,
                    Message = "Main image is required"
                };
            }

            var uploadedResult = await _fileService.UploadAsync(request.MainImage);

            if (!uploadedResult.Success)
            {
                return new Result<ProductResponse>
                {
                    Success = false,
                    Message = uploadedResult.Message
                };
            }

            var product = request.Adapt<Product>();
            product.MainImage = uploadedResult.Data!;

            await _productRepository.CreateAsync(product);

            return new Result<ProductResponse>
            {
                Success = true,
                Message = "Success",
                Data = product.Adapt<ProductResponse>()
            };
        } catch (Exception exception)
        {
            return new Result<ProductResponse>
            {
                Success = false,
                Message = exception.InnerException!.Message
            };
        }
    }
}
