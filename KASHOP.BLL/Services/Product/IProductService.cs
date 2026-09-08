using KASHOP.DAL;

namespace KASHOP.BLL;

public interface IProductService
{
    Task<Result<ProductResponse>> CreateProduct(ProductRequest request);
}
