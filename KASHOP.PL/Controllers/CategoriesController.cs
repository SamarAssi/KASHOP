using KASHOP.DAL;
using KASHOP.PL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _localizer;
        
        public CategoriesController(
            ApplicationDbContext context,
            IStringLocalizer<SharedResources> localizer
        )
        {
            _context = context;
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_localizer["Success"].Value);
        }
    }
}
