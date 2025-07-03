using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTierV2_API._Services.Interfaces.Report;
using eTierV2_API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace eTierV2_API.Controllers.Report
{
    [ApiController]
    [Route("api/[controller]")]
    public class C_2_3_1_Meeting_Audit_ReportController : ControllerBase
    {
        private readonly I_2_3_1_Meeting_Audit_Report _services;
        public C_2_3_1_Meeting_Audit_ReportController(I_2_3_1_Meeting_Audit_Report services)
        {
            _services = services;
        }

        [HttpGet("ExportExcel")]
        public async Task<ActionResult> DownloadExcel([FromQuery] VW_T2_Meeting_LogParam param)
        {
            var result = await _services.DownloadExcel(param);
            return File(result, "application/xlsx", $"Excel_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.xlsx");
        }
    }
}