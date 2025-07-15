using Machine_API._Service.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    public class CategoryController : ApiController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var result = await _categoryService.GetAllCategory();
            return Ok(result);
        }
    }
}