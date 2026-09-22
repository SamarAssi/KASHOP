using KASHOP.DAL;

namespace KASHOP.BLL;

public interface ICartService
{
    Task<Result<bool>> AddToCart(string userId, CartItemRequest request);
}
