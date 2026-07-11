using System.Data;
using System.Threading.Tasks;
using KASHOP.BLL;
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
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        
        public CategoriesController(
            ICategoryService categoryService,
            IStringLocalizer<SharedResources> localizer
        )
        {
            _categoryService = categoryService;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategories();

            return Ok(new { _localizer["Success"].Value, categories });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetCategory(category => category.Id == id);

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
            var category = await _categoryService.CreateCategory(request);

            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryRequest request)
        {
            var updated = await _categoryService.UpdateCategory(id, request);

            return !updated ? BadRequest() : Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryService.DeleteCategory(id);

            return !deleted ? BadRequest() : Ok();
        }
    }
}
