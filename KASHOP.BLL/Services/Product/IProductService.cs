using System.Linq.Expressions;
using KASHOP.DAL;

namespace KASHOP.BLL;

public interface IProductService
{
    Task<Result<ProductResponse>> CreateProduct(ProductRequest request);
    Task<Result<List<ProductResponse>>> GetAllProducts();
    Task<Result<ProductResponse>> GetProduct(Expression<Func<Product, bool>> filter);
}
