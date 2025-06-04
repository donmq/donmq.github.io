using API._Services.Interfaces.Manage;
using API.Dtos.Manage.CategoryManagement;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.Manage
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParam param)
        {
            var data = await _categoryService.GetAll(param);
            return Ok(data);
        }

        [HttpGet("GetEditDetail")]
        public async Task<IActionResult> GetEditDetail([FromQuery] int id)
        {
            var data = await _categoryService.GetEditDetail(id);
            return Ok(data);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CategoryDetailDto category)
        {
            var data = await _categoryService.Create(category);
            return Ok(data);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CategoryDetailDto category)
        {
            var data = await _categoryService.Update(category);
            return Ok(data);
        }
    }
}