using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.EmployeeMaintenance
{
    public class C_4_1_15_ContractManagement : APIController
    {
        private readonly I_4_1_15_ContractManagement _service;
        public C_4_1_15_ContractManagement(I_4_1_15_ContractManagement service)
        {
            _service = service;
        }

        [HttpGet("GetData")]
        public async Task<ActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] ContractManagementParam param)
        {
            param.Username = userName;
            var data = await _service.GetData(pagination, param, roleList);
            return Ok(data);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ContractManagementDto data)
        {
            data.Update_By = userName;
            data.Update_Time = DateTime.Now;
            var result = await _service.Create(data);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromBody] ContractManagementDto data)
        {
            data.Update_By = userName;
            data.Update_Time = DateTime.Now;
            var result = await _service.Update(data);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] ContractManagementDto data)
        {
            var result = await _service.Delete(data);
            return Ok(result);
        }

        [HttpGet("GetListDepartment")]
        public async Task<IActionResult> GetListDepartment(string division, string factory, string lang)
        {
            return Ok(await _service.GetListDepartment(division, factory, lang));
        }

        [HttpGet("GetListDivision")]
        public async Task<IActionResult> GetListDivision(string lang)
        {
            return Ok(await _service.GetListDivision(lang));
        }

        [HttpGet("GetListFactory")]
        public async Task<IActionResult> GetListFactory(string division, string lang)
        {
            return Ok(await _service.GetListFactory(division, lang));
        }

        [HttpGet("GetListContractType")]
        public async Task<IActionResult> GetListContractType(string division, string factory, string lang)
        {
            return Ok(await _service.GetListContractType(division, factory, lang));
        }

        [HttpGet("GetListAssessmentResult")]
        public async Task<IActionResult> GetListAssessmentResult(string lang)
        {
            return Ok(await _service.GetListAssessmentResult(lang));
        }
        [HttpGet("GetPerson")]
        public async Task<IActionResult> GetPerson([FromQuery] PersonalParam paramPersonal)
        {
            return Ok(await _service.GetPerson(paramPersonal));
        }
        [HttpGet("GetProbationDate")]
        public async Task<IActionResult> GetProbationDate(string division, string factory, string contractType)
        {
            return Ok(await _service.GetProbationDate(division, factory, contractType));
        }
        [HttpGet("GetEmployeeID")]
        public async Task<IActionResult> GetEmployeeID()
        {
            return Ok(await _service.GetEmployeeID());
        }
    }
}