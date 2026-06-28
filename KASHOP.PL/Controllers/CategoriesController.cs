using KASHOP.DAL;
using KASHOP.PL;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var categories = _context.Categories
                .Include(category => category.Translations)
                .AsNoTracking()
                .ToList();

            var response = categories.Adapt<List<CategoryResponse>>();

            return Ok(new { _localizer["Success"].Value, response });
        }

        [HttpPost]
        public IActionResult Create(CategoryRequest request)
        {
            var category = request.Adapt<Category>();

            _context.Add(category);
            _context.SaveChanges();

            return Ok(request);
        }
    }
}
