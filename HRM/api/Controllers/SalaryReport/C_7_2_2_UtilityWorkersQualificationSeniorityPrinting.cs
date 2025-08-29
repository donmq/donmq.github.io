using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API._Services.Interfaces.SalaryReport;
using API.DTOs.SalaryMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.SalaryReport
{
    public class C_7_2_2_UtilityWorkersQualificationSeniorityPrinting : APIController
    {
        private readonly I_7_2_2_UtilityWorkersQualificationSeniorityPrinting _service;
        public C_7_2_2_UtilityWorkersQualificationSeniorityPrinting(I_7_2_2_UtilityWorkersQualificationSeniorityPrinting service)
        {
            _service = service;
        }
        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string language)
        {
            return Ok(await _service.GetListFactory(language, userName));
        }
        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string language, string factory)
        {
            return Ok(await _service.GetListDepartment(language, factory));
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery]UtilityWorkersQualificationSeniorityPrintingParam param)
        {
            return Ok(await _service.Search(param));
        }
        [HttpGet("DownloadFileExcel")]
        public async Task<IActionResult> DownloadFileExcel([FromQuery]UtilityWorkersQualificationSeniorityPrintingParam param)
        {
            return Ok(await _service.DownloadFileExcel(param, userName));
        }
    }
}