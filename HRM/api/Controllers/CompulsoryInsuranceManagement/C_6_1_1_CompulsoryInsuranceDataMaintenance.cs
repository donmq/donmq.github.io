
using API._Services.Interfaces.CompulsoryInsuranceManagement;
using API.DTOs.CompulsoryInsuranceManagement;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.CompulsoryInsuranceManagement
{
    public class C_6_1_1_CompulsoryInsuranceDataMaintenance : APIController
    {
        private readonly I_6_1_1_CompulsoryInsuranceDataMaintenance _service;
        public C_6_1_1_CompulsoryInsuranceDataMaintenance(I_6_1_1_CompulsoryInsuranceDataMaintenance service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] CompulsoryInsuranceDataMaintenanceParam param)
        {
            var result = await _service.GetDataPagination(pagination, param);
            return Ok(result);
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language, roleList));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CompulsoryInsuranceDataMaintenanceDto dto)
        {
            var result = await _service.Create(dto);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CompulsoryInsuranceDataMaintenanceDto dto)
        {
            var result = await _service.Update(dto);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] CompulsoryInsuranceDataMaintenanceDto dto)
        {
            var result = await _service.Delete(dto);
            return Ok(result);
        }
        [HttpGet("GetListInsuranceType")]
        public async Task<IActionResult> GetListInsuranceType(string language)
        {
            return Ok(await _service.GetListInsuranceType(language));
        }
        [HttpGet("DownloadFileExcel")]
        public async Task<IActionResult> Download([FromQuery] CompulsoryInsuranceDataMaintenanceParam param)
        {
            var result = await _service.DownloadFileExcel(param, userName);
            return Ok(result);
        }

        [HttpPost("UploadFileExcel")]
        public async Task<IActionResult> UploadFileExcel([FromForm] CompulsoryInsuranceDataMaintenance_Upload param)
        {
            return Ok(await _service.UploadFileExcel(param, roleList, userName));
        }

        [HttpGet("DownloadFileTemplate")]
        public async Task<IActionResult> DownloadFileTemplate()
        {
            var result = await _service.DownloadFileTemplate();
            return Ok(result);
        }
    }
}