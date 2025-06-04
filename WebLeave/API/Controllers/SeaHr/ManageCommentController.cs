using System.Security.Claims;
using API._Services.Interfaces.SeaHr;
using API.Dtos.Common;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.SeaHr
{
    public class ManageCommentController : ApiController
    {
        private readonly IManageCommentArchiveService _service;

        public ManageCommentController(IManageCommentArchiveService service)
        {
            _service = service;
        }

        private string GetUser()
        {
            return User.FindFirst(ClaimTypes.Name).Value;
        }

        [HttpGet("Search")]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam param)
        {
            var result = await _service.GetDataPagination(param);
            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CommentArchiveDto commentArchiveDto)
        {
            string username = GetUser();
            commentArchiveDto.CreateTime = DateTime.Now;
            var result = await _service.Add(commentArchiveDto, username);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int commentArchiveID)
        {
            var result = await _service.Delete(commentArchiveID);
            return Ok(result);
        }

    }
}