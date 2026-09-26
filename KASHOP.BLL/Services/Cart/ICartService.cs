using KASHOP.DAL;

namespace KASHOP.BLL;

public interface ICartService
{
    Task<Result<bool>> AddToCart(string userId, CartItemRequest request);
    Task<Result<List<CartItemResponse>>> GetCart(string userId);
    Task<Result<bool>> RemoveFromCart(string userId, int productId);
    Task<Result<bool>> UpdateQuantity(string userId, int productId, int count);
    Task<Result<bool>> ClearCart(string userId);
}
