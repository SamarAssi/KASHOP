using KASHOP.BLL;
using KASHOP.DAL;
using KASHOP.PL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : BaseApiController
    {
        private readonly ICartService _cartService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CartsController(
            ICartService cartService,
            IStringLocalizer<SharedResources> localizer
        )
        {
            _cartService = cartService;
            _localizer = localizer;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(CartItemRequest request)
        {
            var result = await _cartService.AddToCart(CurrentUserId, request);

            return result.Success ?
                Ok(result) :
                BadRequest(result);
        }
    }
}
