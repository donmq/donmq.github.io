using Machine_API._Service.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Machine_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadDataHPA15Controller : ControllerBase
    {
        private readonly IUploadDataHPA15Service _service;

        public UploadDataHPA15Controller(IUploadDataHPA15Service service)
        {
            _service = service;
        }

        [HttpPost("ImportDataExcel")]
        public async Task<IActionResult> ImportDataExcel([FromForm] IFormFile file)
        {
            var result = await _service.ImportDataExcel(file);
            return Ok(result);
        }
    }
}