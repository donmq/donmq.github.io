using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AttendanceMaintenance
{
    public class C_5_1_26_FemaleEmployeeMenstrualLeaveHoursMaintenance : APIController
    {
        private readonly I_5_1_26_FemaleEmployeeMenstrualLeaveHoursMaintenance _service;

        public C_5_1_26_FemaleEmployeeMenstrualLeaveHoursMaintenance(I_5_1_26_FemaleEmployeeMenstrualLeaveHoursMaintenance service)
        {
            _service = service;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] FemaleEmpMenstrualMain data)
        {
            return Ok(await _service.Add(data, userName));
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromBody] FemaleEmpMenstrualMain data)
        {
            return Ok(await _service.Edit(data, userName));
        }
        [HttpGet("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] FemaleEmpMenstrualParam param)
        {
            var result = await _service.DownloadExcel(param, userName);
            return Ok(result);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] FemaleEmpMenstrualMain data)
            => Ok(await _service.Delete(data));

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory([FromQuery] string language)
            => Ok(await _service.GetListFactory(language));

        [HttpGet("GetListFactoryAdd")]
        public async Task<IActionResult> GetListFactoryAdd([FromQuery] string language)
            => Ok(await _service.GetListFactoryAdd(userName, language));

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment([FromQuery] string factory, [FromQuery] string language)
            => Ok(await _service.GetListDepartment(factory, language));

        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] FemaleEmpMenstrualParam param)
            => Ok(await _service.GetDataPagination(pagination, param));

        [HttpGet("GetListFactoryByUser")]
        public async Task<IActionResult> GetListFactoryByUser(string language)
        {
            return Ok(await _service.GetListFactoryByUser(language, userName));
        }
    }
}