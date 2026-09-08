using System.Data;
using System.Threading.Tasks;
using KASHOP.BLL;
using KASHOP.DAL;
using KASHOP.PL;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            var result = await _categoryService.GetAllCategories();

            return result.Success ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryService.GetCategory(category => category.Id == id);

            return result.Success ?
                Ok(result) :
                NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
            var result = await _categoryService.CreateCategory(request);

            return result.Success ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryRequest request)
        {
            var result = await _categoryService.UpdateCategory(id, request);

            return result.Success ?
                Ok(result) :
                NotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategory(id);

            return result.Success ?
                Ok(result) :
                NotFound(result);
        }
    }
}
