using API._Services.Interfaces.Manage;
using API.Dtos.Manage.GroupBaseManage;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Manage
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupBaseController : ControllerBase
    {
        private readonly IGroupBaseService _groupBaseService;

        public GroupBaseController(IGroupBaseService groupBaseService)
        {
            _groupBaseService = groupBaseService;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData()
        {
            var data = await _groupBaseService.GetGroupBaseData();
            return Ok(data);
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] GroupBaseTitleExcel title)
        {
            var result = await _groupBaseService.ExportExcel(title);
            return Ok(result);
        }

        [HttpPost("AddGroup")]
        public async Task<IActionResult> AddGroup([FromBody] GroupBaseAndGroupLangDto GroupBaseAndGroupLang)
        {
            var data = await _groupBaseService.AddGroup(GroupBaseAndGroupLang);
            return Ok(data);
        }

        [HttpPut("EditGroup")]
        public async Task<IActionResult> EditGroup([FromBody] GroupBaseAndGroupLangDto GroupBaseAndGroupLang)
        {
            var data = await _groupBaseService.EditGroup(GroupBaseAndGroupLang);
            return Ok(data);
        }

        [HttpGet("GetDetailGroupBase")]
        public async Task<IActionResult> GetDetailGroupBase([FromQuery] int IDGroupBase)
        {
            var data = await _groupBaseService.GetDetailGroupBase(IDGroupBase);
            return Ok(data);
        }

         [HttpDelete("RemoveGroup")]
        public async Task<IActionResult> RemoveGroup([FromQuery] int GBID)
        {
            var data = await _groupBaseService.RemoveGroup(GBID);
            return Ok(data);
        }
    }
}