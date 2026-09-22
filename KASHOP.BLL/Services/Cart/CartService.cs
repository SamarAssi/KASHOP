using KASHOP.DAL;

namespace KASHOP.BLL;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;

    public CartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<bool>> AddToCart(string userId, CartItemRequest request)
    {
        var product = await _unitOfWork.ProductRepository.GetOneAsync(
            p => p.Id == request.ProductId
        );

        if (product is null)
        {
            return Result<bool>.Fail("Product Not Found");
        }

        if (request.Count < 0)
        {
            return Result<bool>.Fail("Count must be greater than zero");
        }

        if (product.Quantity < request.Count)
        {
            return Result<bool>.Fail("Not enough stock available");
        }

        var existingItem = await _unitOfWork.CartRepository.GetOneAsync(
            cart => cart.UserId == userId && cart.ProductId == request.ProductId
        );

        if (existingItem is not null)
        {
            existingItem.Count += request.Count;
        } else
        {
            var newCartItem = new CartItem
            {
                UserId = userId,
                ProductId = request.ProductId,
                Count = request.Count
            };

            await _unitOfWork.CartRepository.CreateAsync(newCartItem);
        }

        await _unitOfWork.CompleteAsync();

        return Result<bool>.Ok();
    }
}
