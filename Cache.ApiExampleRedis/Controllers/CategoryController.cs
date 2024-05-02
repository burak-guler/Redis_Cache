using Cache.ApiExampleRedis.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Cache.ApiExampleRedis.Controllers
{
    [Route("api")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("category/getall")]
        public IActionResult GetAll()
        {
            return Ok(_categoryService.GetAllCategory());
        }
    }
}
