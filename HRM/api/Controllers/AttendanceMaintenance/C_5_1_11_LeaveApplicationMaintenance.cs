using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_11_LeaveApplicationMaintenance : APIController
    {
        private readonly I_5_1_11_LeaveApplicationMaintenance _service;
        public C_5_1_11_LeaveApplicationMaintenance(I_5_1_11_LeaveApplicationMaintenance service)
        {
            _service = service;
        }
        [HttpGet("GetDropDownList")]
        public async Task<IActionResult> GetDropDownList([FromQuery] LeaveApplicationMaintenance_Param param)
        {
            var result = await _service.GetDropDownList(param, roleList);
            return Ok(result);
        }
        [HttpGet("GetSearchDetail")]
        public async Task<IActionResult> GetSearchDetail([FromQuery] PaginationParam param, [FromQuery] LeaveApplicationMaintenance_Param filter)
        {
            var result = await _service.GetSearchDetail(param, filter);
            return Ok(result);
        }
        [HttpGet("IsExistedData")]
        public async Task<IActionResult> IsExistedData([FromQuery] LeaveApplicationMaintenance_Param param)
        {
            return Ok(await _service.IsExistedData(param));
        }
        [HttpPost("PostData")]
        public async Task<IActionResult> PostData([FromBody] LeaveApplicationMaintenance_Main data)
        {
            var result = await _service.PostData(data, userName);
            return Ok(result);
        }
        [HttpPut("PutData")]
        public async Task<IActionResult> PutData([FromBody] LeaveApplicationMaintenance_Main data)
        {
            var result = await _service.PutData(data, userName);
            return Ok(result);
        }
        [HttpDelete("DeleteData")]
        public async Task<IActionResult> DeleteData(LeaveApplicationMaintenance_Main data)
        {
            var result = await _service.DeleteData(data);
            return Ok(result);
        }
        [HttpGet("DownloadExcelTemplate")]
        public async Task<ActionResult> DownloadExcelTemplate()
        {
            var result = await _service.DownloadExcelTemplate();
            return Ok(result);
        }
        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            var result = await _service.UploadExcel(file, roleList, userName);
            return Ok(result);
        }
        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> Export([FromQuery] LeaveApplicationMaintenance_Param param)
        {
            param.UserName = userName;
            var result = await _service.ExportExcel(param);
            return Ok(result);
        }
    }
}