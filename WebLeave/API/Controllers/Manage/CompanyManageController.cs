using API._Services.Interfaces.Manage;
using API.Dtos.Manage.CompanyManage;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Manage
{
    public class CompanyManageController : ApiController
    {
        private readonly ICompanyManageService _companyManageService;

        public CompanyManageController(ICompanyManageService companyManageService)
        {
            _companyManageService = companyManageService;
        }

        [HttpGet("GetAllCompany")]
        public async Task<IActionResult> GetAllCompany()
        {
            var data = await _companyManageService.GetAllCompany();
            return Ok(data);
        }

        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompany([FromBody] CompanyManageDto CompanyAdd)
        {
            var data = await _companyManageService.AddCompany(CompanyAdd);
            return Ok(data);
        }

        [HttpPut("EditCompany")]
        public async Task<IActionResult> EditCompany([FromBody] CompanyManageDto CompanyEdit)
        {
            var data = await _companyManageService.EditCompany(CompanyEdit);
            return Ok(data);
        }
    }
}