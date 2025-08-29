using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers.SalaryMaintenance
{
    public class C_7_1_26_FinSalaryCloseMaintenanceController : APIController
    {
        private readonly I_7_1_26_FinSalaryCloseMaintenance _service;
        public C_7_1_26_FinSalaryCloseMaintenanceController(I_7_1_26_FinSalaryCloseMaintenance service)
        {
            _service = service;
        }

        [HttpGet("DownloadExcel")]
        public async Task<IActionResult> DownloadExcel([FromQuery] FinSalaryCloseMaintenance_Param param)
        {
            return Ok(await _service.DownLoadExcel(param, userName));
        }

        [HttpGet("GetDataPaination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] FinSalaryCloseMaintenance_Param param)
        {
            return Ok(await _service.GetDataPagination(pagination, param));
        }

        [HttpGet("GetDepartment")]
        public async Task<IActionResult> GetListDepartment([FromQuery] string factory, [FromQuery] string language)
        {
            return Ok(await _service.GetListDepartment(factory, language));
        }    
        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory([FromQuery] string language)
        {
            return Ok(await _service.GetListFactory(userName, language));
        }

        [HttpGet("GetListPermissionGroup")]
        public async Task<IActionResult> GetListPermissionGroup([FromQuery] string factory, [FromQuery] string language)
        {
            return Ok(await _service.GetListPermissionGroup(factory, language));
        }

        [HttpGet("GetListTypeHeadEmployeeID")]
        public async Task<IActionResult> GetListTypeHeadEmployeeID([FromQuery] string factory)
        {
            return Ok(await _service.GetListTypeHeadEmployeeID(factory));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] FinSalaryCloseMaintenance_MainData data)
        {
            data.Update_By = userName;
            return Ok(await _service.Update(data));
        }
        [HttpPut("BatchUpdateData")]
        public async Task<IActionResult> BatchUpdateData([FromBody] BatchUpdateData_Param param)
        {
            return Ok(await _service.BatchUpdateData(param, userName));
        }

    }
}