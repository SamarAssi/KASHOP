using KASHOP.DAL;
using Mapster;
using Microsoft.EntityFrameworkCore.Query;

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

    public async Task<Result<List<CartItemResponse>>> GetCart(string userId)
    {
        var userCartItem = await _unitOfWork.CartRepository.GetAllAsync(
            filter: cart => cart.UserId == userId,
            includes:
            [
                nameof(CartItem.Product),
                $"{nameof(CartItem.Product)}.{nameof(Product.Translations)}"
            ] 
        );

        var response = userCartItem.Adapt<List<CartItemResponse>>();

        return Result<List<CartItemResponse>>.Ok("Success", response);
    }

    public async Task<Result<bool>> RemoveFromCart(string userId, int productId)
    {
        var cartItem = await _unitOfWork.CartRepository.GetOneAsync(
            cart => cart.UserId == userId && cart.ProductId == productId
        );

        if (cartItem is null)
        {
            return Result<bool>.Fail("Cart Item Not Found");
        }

        _unitOfWork.CartRepository.DeleteAsync(cartItem);

        var affectedRows = await _unitOfWork.CompleteAsync();

        return affectedRows > 0 ?
            Result<bool>.Ok() :
            Result<bool>.Fail("Fail to Remove from Cart");
    }

    public async Task<Result<bool>> UpdateQuantity(string userId, int productId, int count)
    {
        if (count <= 0)
        {
            return Result<bool>.Fail("Count must be greater than zero");
        }

        var cartItem = await _unitOfWork.CartRepository.GetOneAsync(
            filter: cart => cart.UserId == userId && cart.ProductId == productId,
            includes:
            [
                nameof(CartItem.Product)
            ]
        );

        if (cartItem.Product.Quantity < count)
        {
            return Result<bool>.Fail("Not enough stock available");
        }

        cartItem.Count = count;

        var affectedRows = await _unitOfWork.CompleteAsync();

        return affectedRows > 0 ?
            Result<bool>.Ok() :
            Result<bool>.Fail("Failed to Update Quantity");
    }
}
